using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMS.EntityFramework.Migrations
{
    public partial class LogIUpdaten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
<<<<<<< HEAD
            // Drop default constraint on ChangedByName BEFORE dropping the column
            migrationBuilder.Sql(
                "DECLARE @con NVARCHAR(256);" +
                "SELECT @con = d.name " +
                "FROM sys.default_constraints d " +
                "INNER JOIN sys.columns c ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id " +
                "INNER JOIN sys.tables t ON c.object_id = t.object_id " +
                "WHERE t.name = 'Departments' AND c.name = 'ChangedByName';" +
                "IF @con IS NOT NULL EXEC('ALTER TABLE [Departments] DROP CONSTRAINT [' + @con + ']');"
            );

            // Drop the column
            migrationBuilder.Sql(
                "IF EXISTS (" +
                "    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS " +
                "    WHERE TABLE_NAME = 'Departments' AND COLUMN_NAME = 'ChangedByName'" +
                ") ALTER TABLE [Departments] DROP COLUMN [ChangedByName];"
            );
=======
            // Drop ChangedByName from Departments if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Departments' AND COLUMN_NAME = 'ChangedByName'
                )
                BEGIN
                    DECLARE @dfName nvarchar(128);
                    SELECT @dfName = dc.name
                    FROM sys.default_constraints dc
                    INNER JOIN sys.columns c
                        ON c.default_object_id = dc.object_id
                    INNER JOIN sys.tables t
                        ON t.object_id = c.object_id
                    WHERE t.name = 'Departments' AND c.name = 'ChangedByName';

                    IF @dfName IS NOT NULL
                    BEGIN
                        EXEC('ALTER TABLE [Departments] DROP CONSTRAINT [' + @dfName + ']');
                    END

                    ALTER TABLE [Departments] DROP COLUMN [ChangedByName]
                END
            ");
>>>>>>> 54b492ec9b324349f7ad530acd422e0b79b45847

            // Drop Logins table if it exists
            migrationBuilder.Sql(
                "IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Logins') " +
                "DROP TABLE [Logins];"
            );

            // Add PasswordHash to Staffs
            migrationBuilder.Sql(
                "IF NOT EXISTS (" +
                "    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS " +
                "    WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordHash'" +
                ") ALTER TABLE [Staffs] ADD [PasswordHash] varbinary(max) NOT NULL DEFAULT 0x;"
            );

            // Add PasswordSalt to Staffs
            migrationBuilder.Sql(
                "IF NOT EXISTS (" +
                "    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS " +
                "    WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordSalt'" +
                ") ALTER TABLE [Staffs] ADD [PasswordSalt] varbinary(max) NOT NULL DEFAULT 0x;"
            );

            // Create VisitorLogs table
<<<<<<< HEAD
            // FK to Appointments uses NO ACTION to avoid cascade cycle:
            // VisitorLogs->Visitors (CASCADE) + VisitorLogs->Appointments->Visitors = multiple paths
            migrationBuilder.Sql(
                "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitorLogs') " +
                "BEGIN " +
                "    CREATE TABLE [VisitorLogs] (" +
                "        [Id]            int IDENTITY(1,1) NOT NULL," +
                "        [VisitorId]     int NOT NULL," +
                "        [StaffId]       int NOT NULL," +
                "        [AppointmentId] int NULL," +
                "        [CheckInTime]   datetime2 NOT NULL," +
                "        [CheckOutTime]  datetime2 NULL," +
                "        [Remarks]       nvarchar(500) NULL," +
                "        [ChangedBy]     int NOT NULL," +
                "        CONSTRAINT [PK_VisitorLogs] PRIMARY KEY ([Id])," +
                "        CONSTRAINT [FK_VisitorLogs_Visitors_VisitorId] FOREIGN KEY ([VisitorId]) REFERENCES [Visitors]([Id]) ON DELETE CASCADE," +
                "        CONSTRAINT [FK_VisitorLogs_Staffs_StaffId] FOREIGN KEY ([StaffId]) REFERENCES [Staffs]([Id]) ON DELETE NO ACTION," +
                "        CONSTRAINT [FK_VisitorLogs_Appointments_AppointmentId] FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments]([Id]) ON DELETE NO ACTION" +
                "    );" +
                "    CREATE INDEX [IX_VisitorLogs_VisitorId]     ON [VisitorLogs]([VisitorId]);" +
                "    CREATE INDEX [IX_VisitorLogs_StaffId]       ON [VisitorLogs]([StaffId]);" +
                "    CREATE INDEX [IX_VisitorLogs_AppointmentId] ON [VisitorLogs]([AppointmentId]);" +
                "END"
            );
=======
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitorLogs')
                BEGIN
                    CREATE TABLE [VisitorLogs] (
                        [Id]            int IDENTITY(1,1) NOT NULL,
                        [VisitorId]     int NOT NULL,
                        [StaffId]       int NOT NULL,
                        [AppointmentId] int NULL,
                        [CheckInTime]   datetime2 NOT NULL,
                        [CheckOutTime]  datetime2 NULL,
                        [Remarks]       nvarchar(500) NULL,
                        [ChangedBy]     int NOT NULL,
                        CONSTRAINT [PK_VisitorLogs] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_VisitorLogs_Visitors_VisitorId]
                            FOREIGN KEY ([VisitorId]) REFERENCES [Visitors]([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_VisitorLogs_Staffs_StaffId]
                            FOREIGN KEY ([StaffId]) REFERENCES [Staffs]([Id]),
                        CONSTRAINT [FK_VisitorLogs_Appointments_AppointmentId]
                            FOREIGN KEY ([AppointmentId]) REFERENCES [Appointments]([Id]) ON DELETE NO ACTION
                    );
                    CREATE INDEX [IX_VisitorLogs_VisitorId]     ON [VisitorLogs]([VisitorId]);
                    CREATE INDEX [IX_VisitorLogs_StaffId]       ON [VisitorLogs]([StaffId]);
                    CREATE INDEX [IX_VisitorLogs_AppointmentId] ON [VisitorLogs]([AppointmentId]);
                END
            ");
>>>>>>> 54b492ec9b324349f7ad530acd422e0b79b45847
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitorLogs') DROP TABLE [VisitorLogs];");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordHash') ALTER TABLE [Staffs] DROP COLUMN [PasswordHash];");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordSalt') ALTER TABLE [Staffs] DROP COLUMN [PasswordSalt];");
        }
    }
}
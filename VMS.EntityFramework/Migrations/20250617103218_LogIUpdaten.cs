//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace VMS.EntityFramework.Migrations
//{
//    /// <inheritdoc />
//    public partial class LogIUpdaten : Migration
//    {
//        /// <inheritdoc />
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {

//        }

//        /// <inheritdoc />
//        protected override void Down(MigrationBuilder migrationBuilder)
//        {

//        }
//    }
//}


using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMS.EntityFramework.Migrations
{
    public partial class LogIUpdaten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            // Drop Logins table if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Logins')
                BEGIN
                    DROP TABLE [Logins]
                END
            ");

            // Add PasswordHash to Staffs
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordHash'
                )
                BEGIN
                    ALTER TABLE [Staffs] ADD [PasswordHash] varbinary(max) NOT NULL DEFAULT 0x
                END
            ");

            // Add PasswordSalt to Staffs
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Staffs' AND COLUMN_NAME = 'PasswordSalt'
                )
                BEGIN
                    ALTER TABLE [Staffs] ADD [PasswordSalt] varbinary(max) NOT NULL DEFAULT 0x
                END
            ");

            // Create VisitorLogs table
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitorLogs') DROP TABLE [VisitorLogs]");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Staffs' AND COLUMN_NAME='PasswordHash') ALTER TABLE [Staffs] DROP COLUMN [PasswordHash]");
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='Staffs' AND COLUMN_NAME='PasswordSalt') ALTER TABLE [Staffs] DROP COLUMN [PasswordSalt]");
        }
    }
}
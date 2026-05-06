using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMS.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddChangedByNameToDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitorLogs_Appointments_AppointmentId",
                table: "VisitorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorLogs_Staffs_StaffId",
                table: "VisitorLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "VisitorLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedByName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorLogs_Appointments_AppointmentId",
                table: "VisitorLogs",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorLogs_Staffs_StaffId",
                table: "VisitorLogs",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitorLogs_Appointments_AppointmentId",
                table: "VisitorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorLogs_Staffs_StaffId",
                table: "VisitorLogs");

            migrationBuilder.DropColumn(
                name: "ChangedByName",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "VisitorLogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorLogs_Appointments_AppointmentId",
                table: "VisitorLogs",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorLogs_Staffs_StaffId",
                table: "VisitorLogs",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

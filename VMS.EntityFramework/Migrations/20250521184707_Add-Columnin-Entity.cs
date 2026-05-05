using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMS.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddColumninEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedByName",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedByName",
                table: "Departments");
        }
    }
}

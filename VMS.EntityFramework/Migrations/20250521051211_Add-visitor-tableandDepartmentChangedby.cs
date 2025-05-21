﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMS.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddvisitortableandDepartmentChangedby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChangedBy",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Departments");
        }
    }
}

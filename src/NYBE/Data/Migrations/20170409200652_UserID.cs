using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NYBE.Data.Migrations
{
    public partial class UserID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "PendingSchools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "PendingBooks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "PendingSchools");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "PendingBooks");
        }
    }
}

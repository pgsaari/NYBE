using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NYBE.Data.Migrations
{
    public partial class somethingIsBrokenButIDontKnowWhat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditListingViewModelID",
                table: "Courses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EditListingViewModelID",
                table: "Courses",
                column: "EditListingViewModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_EditListingViewModel_EditListingViewModelID",
                table: "Courses",
                column: "EditListingViewModelID",
                principalTable: "EditListingViewModel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_EditListingViewModel_EditListingViewModelID",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_EditListingViewModelID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EditListingViewModelID",
                table: "Courses");
        }
    }
}

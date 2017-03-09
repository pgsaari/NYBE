using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NYBE.Data.Migrations
{
    public partial class UserIDTypetoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_AspNetUsers_UserId",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_UserId",
                table: "BookListings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookListings");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserID",
                table: "BookListings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_ApplicationUserID",
                table: "BookListings",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_AspNetUsers_ApplicationUserID",
                table: "BookListings",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_AspNetUsers_ApplicationUserID",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_ApplicationUserID",
                table: "BookListings");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserID",
                table: "BookListings",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookListings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_UserId",
                table: "BookListings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_AspNetUsers_UserId",
                table: "BookListings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

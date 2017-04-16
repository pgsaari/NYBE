using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NYBE.Data.Migrations
{
    public partial class whatishappening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_EditListingViewModel_EditListingViewModelID",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "EditListingViewModel");

            migrationBuilder.DropIndex(
                name: "IX_Courses_EditListingViewModelID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EditListingViewModelID",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EditListingViewModelID",
                table: "Courses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EditListingViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bookID = table.Column<int>(nullable: true),
                    condition = table.Column<string>(nullable: true),
                    courseID = table.Column<int>(nullable: false),
                    price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditListingViewModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EditListingViewModel_Books_bookID",
                        column: x => x.bookID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditListingViewModel_Courses_courseID",
                        column: x => x.courseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EditListingViewModelID",
                table: "Courses",
                column: "EditListingViewModelID");

            migrationBuilder.CreateIndex(
                name: "IX_EditListingViewModel_bookID",
                table: "EditListingViewModel",
                column: "bookID");

            migrationBuilder.CreateIndex(
                name: "IX_EditListingViewModel_courseID",
                table: "EditListingViewModel",
                column: "courseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_EditListingViewModel_EditListingViewModelID",
                table: "Courses",
                column: "EditListingViewModelID",
                principalTable: "EditListingViewModel",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

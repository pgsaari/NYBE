using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NYBE.Data.Migrations
{
    public partial class ModifyTransactionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "TransactionLogs",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SoldPrice",
                table: "TransactionLogs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "TransactionLogs");

            migrationBuilder.DropColumn(
                name: "SoldPrice",
                table: "TransactionLogs");
        }
    }
}

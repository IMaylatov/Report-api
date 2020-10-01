using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class TemplateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "value",
                table: "templates");

            migrationBuilder.AddColumn<byte[]>(
                name: "data",
                table: "templates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "templates");

            migrationBuilder.AddColumn<byte[]>(
                name: "value",
                table: "templates",
                type: "bytea",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class DeleteReportData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "reports");

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "variables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "variables");

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "reports",
                type: "text",
                nullable: true);
        }
    }
}

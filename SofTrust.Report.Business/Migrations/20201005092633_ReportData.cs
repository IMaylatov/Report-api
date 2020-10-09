using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class ReportData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "reports");
        }
    }
}

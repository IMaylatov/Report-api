using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class OneReportmanyTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports");

            migrationBuilder.DropIndex(
                name: "ix_reports_template_id",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "template_id",
                table: "reports");

            migrationBuilder.AddColumn<int>(
                name: "report_id",
                table: "templates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_templates_report_id",
                table: "templates",
                column: "report_id");

            migrationBuilder.AddForeignKey(
                name: "fk_templates_reports_report_id",
                table: "templates",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_templates_reports_report_id",
                table: "templates");

            migrationBuilder.DropIndex(
                name: "ix_templates_report_id",
                table: "templates");

            migrationBuilder.DropColumn(
                name: "report_id",
                table: "templates");

            migrationBuilder.AddColumn<int>(
                name: "template_id",
                table: "reports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_reports_template_id",
                table: "reports",
                column: "template_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports",
                column: "template_id",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class NullableTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports");

            migrationBuilder.AlterColumn<int>(
                name: "template_id",
                table: "reports",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports",
                column: "template_id",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports");

            migrationBuilder.AlterColumn<int>(
                name: "template_id",
                table: "reports",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_reports_templates_template_id",
                table: "reports",
                column: "template_id",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

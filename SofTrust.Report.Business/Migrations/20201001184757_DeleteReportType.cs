using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SofTrust.Report.Business.Migrations
{
    public partial class DeleteReportType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reports_report_types_type_id",
                table: "reports");

            migrationBuilder.DropTable(
                name: "report_types");

            migrationBuilder.DropIndex(
                name: "ix_reports_type_id",
                table: "reports");

            migrationBuilder.DropColumn(
                name: "type_id",
                table: "reports");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "reports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "reports");

            migrationBuilder.AddColumn<int>(
                name: "type_id",
                table: "reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "report_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_types", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "report_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Malibu" },
                    { 2, "ClosedXml" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_reports_type_id",
                table: "reports",
                column: "type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reports_report_types_type_id",
                table: "reports",
                column: "type_id",
                principalTable: "report_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

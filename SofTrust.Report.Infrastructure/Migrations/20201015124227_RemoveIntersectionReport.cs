using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Infrastructure.Migrations
{
    public partial class RemoveIntersectionReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_data_sets");

            migrationBuilder.DropTable(
                name: "report_data_sources");

            migrationBuilder.DropTable(
                name: "report_variables");

            migrationBuilder.AddColumn<int>(
                name: "report_id",
                table: "variables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "report_id",
                table: "data_sources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "report_id",
                table: "data_sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_variables_report_id",
                table: "variables",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "ix_data_sources_report_id",
                table: "data_sources",
                column: "report_id");

            migrationBuilder.CreateIndex(
                name: "ix_data_sets_report_id",
                table: "data_sets",
                column: "report_id");

            migrationBuilder.AddForeignKey(
                name: "fk_data_sets_reports_report_id",
                table: "data_sets",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_data_sources_reports_report_id",
                table: "data_sources",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_variables_reports_report_id",
                table: "variables",
                column: "report_id",
                principalTable: "reports",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_data_sets_reports_report_id",
                table: "data_sets");

            migrationBuilder.DropForeignKey(
                name: "fk_data_sources_reports_report_id",
                table: "data_sources");

            migrationBuilder.DropForeignKey(
                name: "fk_variables_reports_report_id",
                table: "variables");

            migrationBuilder.DropIndex(
                name: "ix_variables_report_id",
                table: "variables");

            migrationBuilder.DropIndex(
                name: "ix_data_sources_report_id",
                table: "data_sources");

            migrationBuilder.DropIndex(
                name: "ix_data_sets_report_id",
                table: "data_sets");

            migrationBuilder.DropColumn(
                name: "report_id",
                table: "variables");

            migrationBuilder.DropColumn(
                name: "report_id",
                table: "data_sources");

            migrationBuilder.DropColumn(
                name: "report_id",
                table: "data_sets");

            migrationBuilder.CreateTable(
                name: "report_data_sets",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "integer", nullable: false),
                    data_set_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_data_sets", x => new { x.report_id, x.data_set_id });
                    table.ForeignKey(
                        name: "fk_report_data_sets_data_sets_data_set_id",
                        column: x => x.data_set_id,
                        principalTable: "data_sets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_report_data_sets_reports_report_id",
                        column: x => x.report_id,
                        principalTable: "reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report_data_sources",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "integer", nullable: false),
                    data_source_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_data_sources", x => new { x.report_id, x.data_source_id });
                    table.ForeignKey(
                        name: "fk_report_data_sources_data_sources_data_source_id",
                        column: x => x.data_source_id,
                        principalTable: "data_sources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_report_data_sources_reports_report_id",
                        column: x => x.report_id,
                        principalTable: "reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "report_variables",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "integer", nullable: false),
                    variable_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_variables", x => new { x.report_id, x.variable_id });
                    table.ForeignKey(
                        name: "fk_report_variables_reports_report_id",
                        column: x => x.report_id,
                        principalTable: "reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_report_variables_variables_variable_id",
                        column: x => x.variable_id,
                        principalTable: "variables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_report_data_sets_data_set_id",
                table: "report_data_sets",
                column: "data_set_id");

            migrationBuilder.CreateIndex(
                name: "ix_report_data_sources_data_source_id",
                table: "report_data_sources",
                column: "data_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_report_variables_variable_id",
                table: "report_variables",
                column: "variable_id");
        }
    }
}

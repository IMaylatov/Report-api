using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SofTrust.Report.Business.Migrations
{
    public partial class SourceSetVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_sets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_data_sets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_sources",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_data_sources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variables",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    kind = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_data_sets",
                columns: table => new
                {
                    report_id = table.Column<int>(nullable: false),
                    data_set_id = table.Column<int>(nullable: false)
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
                    report_id = table.Column<int>(nullable: false),
                    data_source_id = table.Column<int>(nullable: false)
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
                    report_id = table.Column<int>(nullable: false),
                    variable_id = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_data_sets");

            migrationBuilder.DropTable(
                name: "report_data_sources");

            migrationBuilder.DropTable(
                name: "report_variables");

            migrationBuilder.DropTable(
                name: "data_sets");

            migrationBuilder.DropTable(
                name: "data_sources");

            migrationBuilder.DropTable(
                name: "variables");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class InitialTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "report_types",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Malibu" },
                    { 2, "ClosedXml" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "report_types",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "report_types",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}

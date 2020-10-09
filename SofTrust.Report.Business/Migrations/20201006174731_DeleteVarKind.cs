using Microsoft.EntityFrameworkCore.Migrations;

namespace SofTrust.Report.Business.Migrations
{
    public partial class DeleteVarKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "kind",
                table: "variables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "kind",
                table: "variables",
                type: "text",
                nullable: true);
        }
    }
}

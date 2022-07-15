using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ct.Interview.Web.Api.Migrations
{
    public partial class AsxListedCompanyDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsxListedCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AsxCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GicsIndustryGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsxListedCompanies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsxListedCompanies");
        }
    }
}

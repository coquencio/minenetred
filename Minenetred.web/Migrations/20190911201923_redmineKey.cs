using Microsoft.EntityFrameworkCore.Migrations;

namespace Minenetred.Web.Migrations
{
    public partial class redmineKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Users",
                newName: "RedmineKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RedmineKey",
                table: "Users",
                newName: "Key");
        }
    }
}
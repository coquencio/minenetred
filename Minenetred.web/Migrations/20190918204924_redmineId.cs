using Microsoft.EntityFrameworkCore.Migrations;

namespace Minenetred.web.Migrations
{
    public partial class redmineId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedmineId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedmineId",
                table: "Users");
        }
    }
}

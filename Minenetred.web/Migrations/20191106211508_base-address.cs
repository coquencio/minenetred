using Microsoft.EntityFrameworkCore.Migrations;

namespace Minenetred.Web.Migrations
{
    public partial class baseaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseUri",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUri",
                table: "Users");
        }
    }
}

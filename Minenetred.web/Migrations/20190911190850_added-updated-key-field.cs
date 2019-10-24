using Microsoft.EntityFrameworkCore.Migrations;

namespace Minenetred.Web.Migrations
{
    public partial class addedupdatedkeyfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLoginDate",
                table: "Users",
                newName: "LastKeyUpdatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastKeyUpdatedDate",
                table: "Users",
                newName: "LastLoginDate");
        }
    }
}
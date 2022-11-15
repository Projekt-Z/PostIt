using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostIt.Web.Migrations
{
    public partial class AccountVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PostItVerified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostItVerified",
                table: "Users");
        }
    }
}

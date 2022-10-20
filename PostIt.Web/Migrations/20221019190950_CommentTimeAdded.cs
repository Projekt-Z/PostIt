using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostIt.Web.Migrations
{
    public partial class CommentTimeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeAdded",
                table: "Comment",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "Comment");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class AddUrlTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Posts",
                maxLength: 200,
                nullable: false,
                defaultValue: "[NOT SET]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Posts");
        }
    }
}

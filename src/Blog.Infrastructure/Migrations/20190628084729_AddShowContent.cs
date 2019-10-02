using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class AddShowContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShowContent",
                table: "Posts",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("UPDATE Posts SET ShowContent = Content");

            migrationBuilder.AlterColumn<string>(
                name: "ShowContent",
                table: "Posts",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowContent",
                table: "Posts");
        }
    }
}

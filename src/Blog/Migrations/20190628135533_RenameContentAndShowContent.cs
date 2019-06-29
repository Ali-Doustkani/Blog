using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class RenameContentAndShowContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowContent",
                table: "Posts",
                newName: "MarkedContent");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "DisplayContent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarkedContent",
                table: "Posts",
                newName: "ShowContent");

            migrationBuilder.RenameColumn(
                name: "DisplayContent",
                table: "Posts",
                newName: "Content");
        }
    }
}

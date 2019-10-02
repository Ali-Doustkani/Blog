using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class PostTitleUniqueness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Posts_Title",
                table: "Posts",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Posts_Title",
                table: "Posts");
        }
    }
}

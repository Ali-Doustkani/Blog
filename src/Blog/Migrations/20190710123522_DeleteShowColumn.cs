using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class DeleteShowColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Show",
                table: "Drafts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Show",
                table: "Drafts",
                nullable: false,
                defaultValue: false);
        }
    }
}

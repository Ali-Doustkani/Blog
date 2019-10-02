using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class RemoveAlternateKeyOfTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Infos_Title",
                table: "Infos");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Infos_Title",
                table: "Infos",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Infos_Title",
                table: "Infos");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Infos_Title",
                table: "Infos",
                column: "Title");
        }
    }
}

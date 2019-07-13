using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class SeparateDraftAndPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Infos_Drafts_Id", table: "Infos");
            migrationBuilder.DropPrimaryKey(name: "PK_Posts", table: "Drafts");
            migrationBuilder.AddPrimaryKey(name: "PK_Drafts", table: "Drafts", column: "Id");
            migrationBuilder.AddForeignKey(
                name: "FK_Infos_Drafts_Id",
                table: "Infos",
                column: "Id",
                principalTable: "Drafts",
                principalColumn: "Id");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Infos_Id",
                        column: x => x.Id,
                        principalTable: "Infos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                "INSERT INTO Posts (Id, Url, Content)" +
                "SELECT Id, UrlTitle, DisplayContent FROM Drafts");

            migrationBuilder.DropColumn(
             name: "DisplayContent",
             table: "Drafts");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Drafts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayContent",
                table: "Drafts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Drafts",
                maxLength: 200,
                nullable: false,
                defaultValue: "[NOT SET]");

            migrationBuilder.Sql(
                "UPDATE T1" +
                "SET T1.DisplayContent = T2.Content, T1.UrlTitle = T2.Url" +
                "FROM Drafts T1" +
                "INNER JOIN Posts T2 ON T1.Id = T2.Id");

            migrationBuilder.DropTable(
             name: "Posts");
        }
    }
}

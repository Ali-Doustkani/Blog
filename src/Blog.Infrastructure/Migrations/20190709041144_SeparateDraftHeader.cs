using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class SeparateDraftHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Infos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<int>(nullable: false, defaultValue: 1),
                    Summary = table.Column<string>(nullable: false),
                    Tags = table.Column<string>(nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infos", x => x.Id);
                    table.UniqueConstraint("AK_Infos_Title", x => x.Title);
                    table.ForeignKey(
                        name: "FK_Infos_Drafts_Id",
                        column: x => x.Id,
                        principalTable: "Drafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                "INSERT INTO Infos (Id, Title, PublishDate, Language, Summary, Tags)" +
                "SELECT Id, Title, PublishDate, Language, Summary, Tags FROM Drafts");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Drafts");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Drafts");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Drafts");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Drafts");

            migrationBuilder.DropUniqueConstraint(
            name: "AK_Posts_Title",
            table: "Drafts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Drafts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Drafts",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Drafts",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Drafts",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Drafts",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Drafts",
                maxLength: 150,
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(
                "UPDATE T1" +
                "SET T1.Language = T2.Language, T1.PublishDate = T2.PublishDate, T1.Summary = T2.Summary, T1.Tags = T2.Tags, T1.Title = T2.Title" +
                "FROM Drafts AS T1" +
                "INNER JOIN Infos AS T2 ON T1.Id = T2.Id");

            migrationBuilder.AlterColumn<int>(
               name: "Language",
               table: "Drafts",
               nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
               name: "PublishDate",
               table: "Drafts",
               nullable: false);

            migrationBuilder.AlterColumn<string>(
               name: "Summary",
               table: "Drafts",
               nullable: false);

            migrationBuilder.AlterColumn<string>(
               name: "Tags",
               table: "Drafts",
               nullable: false);

            migrationBuilder.AlterColumn<string>(
               name: "Title",
               table: "Drafts",
               nullable: false);

            migrationBuilder.AddUniqueConstraint(
               name: "AK_Drafts_Title",
               table: "Drafts",
               column: "Title");

            migrationBuilder.DropTable(
               name: "Infos");
        }
    }
}

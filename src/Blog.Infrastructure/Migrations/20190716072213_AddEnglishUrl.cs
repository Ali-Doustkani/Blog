using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
   public partial class AddEnglishUrl : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.AddColumn<string>(
             name: "EnglishUrl",
             table: "Infos",
             nullable: true);

         migrationBuilder.CreateIndex(
             name: "IX_Infos_EnglishUrl",
             table: "Infos",
             column: "EnglishUrl",
             unique: true,
             filter: "[EnglishUrl] IS NOT NULL");

         migrationBuilder.Sql("UPDATE Infos SET EnglishUrl = Title");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropIndex(
             name: "IX_Infos_EnglishUrl",
             table: "Infos");

         migrationBuilder.DropColumn(
             name: "EnglishUrl",
             table: "Infos");
      }
   }
}

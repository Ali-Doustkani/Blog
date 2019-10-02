using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
   public partial class RemovePostInfo : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         UpPost(migrationBuilder);
         UpDraft(migrationBuilder);
         migrationBuilder.DropTable(
             name: "Infos");
      }

      private void UpPost(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropForeignKey(
        name: "FK_Posts_Infos_Id",
        table: "Posts");

         migrationBuilder.AddColumn<int>(
             name: "Language",
             table: "Posts",
             nullable: true);

         migrationBuilder.AddColumn<DateTime>(
             name: "PublishDate",
             table: "Posts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Summary",
             table: "Posts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Tags",
             table: "Posts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Title",
             table: "Posts",
             nullable: true);

         migrationBuilder.Sql("UPDATE T1 SET " +
            "T1.Language=T2.Language, " +
            "T1.PublishDate=T2.PublishDate, " +
            "T1.Summary=T2.Summary, " +
            "T1.Tags=T2.Tags, " +
            "T1.Title=T2.Title " +
            "FROM Posts T1 INNER JOIN Infos T2 ON T1.Id=T2.Id");

         migrationBuilder.AlterColumn<int>(
          name: "Language",
          table: "Posts",
          nullable: false);

         migrationBuilder.AlterColumn<DateTime>(
             name: "PublishDate",
             table: "Posts",
             nullable: false);

         migrationBuilder.AlterColumn<string>(
             name: "Summary",
             table: "Posts",
             nullable: false);

         migrationBuilder.AlterColumn<string>(
             name: "Tags",
             table: "Posts",
             nullable: false);

         migrationBuilder.AlterColumn<string>(
             name: "Title",
             table: "Posts",
             nullable: false);
      }

      private void UpDraft(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.AlterColumn<string>(
           name: "Content",
           table: "Drafts",
           nullable: true);

         migrationBuilder.AddColumn<string>(
          name: "EnglishUrl",
          table: "Drafts",
          nullable: true);

         migrationBuilder.AddColumn<int>(
             name: "Language",
             table: "Drafts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Summary",
             table: "Drafts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Tags",
             table: "Drafts",
             nullable: true);

         migrationBuilder.AddColumn<string>(
             name: "Title",
             table: "Drafts",
             nullable: true);

         migrationBuilder.Sql("UPDATE T1 " +
            "SET T1.EnglishUrl=T2.EnglishUrl, " +
            "T1.Language=T2.Language, " +
            "T1.Summary=T2.Summary, " +
            "T1.Tags=T2.Tags, " +
            "T1.Title=T2.Title " +
            "FROM Drafts T1 INNER JOIN Infos T2 ON T1.Id=T2.Id");

         migrationBuilder.AlterColumn<int>(
            name: "Language",
            table: "Drafts",
            nullable: false);
      }
   }
}

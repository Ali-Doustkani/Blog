using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
   public partial class InitialDeveloper : Migration
   {
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Developers",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Summary = table.Column<string>(nullable: false),
                Skills = table.Column<string>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Developers", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "SideProjects",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Title = table.Column<string>(nullable: false),
                Content = table.Column<string>(nullable: false),
                DeveloperId = table.Column<int>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_SideProjects", x => x.Id);
                table.ForeignKey(
                       name: "FK_SideProjects_Developers_DeveloperId",
                       column: x => x.DeveloperId,
                       principalTable: "Developers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateTable(
             name: "Experiences",
             columns: table => new
             {
                Id = table.Column<int>(nullable: false)
                     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Title = table.Column<string>(nullable: false),
                StartDate = table.Column<DateTime>(nullable: false),
                EndDate = table.Column<DateTime>(nullable: false),
                Content = table.Column<string>(nullable: false),
                DeveloperId = table.Column<int>(nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Experiences", x => x.Id);
                table.ForeignKey(
                       name: "FK_Experiences_Developers_DeveloperId",
                       column: x => x.DeveloperId,
                       principalTable: "Developers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateIndex(
             name: "IX_SideProjects_DeveloperId",
             table: "SideProjects",
             column: "DeveloperId");

         migrationBuilder.CreateIndex(
             name: "IX_Experiences_DeveloperId",
             table: "Experiences",
             column: "DeveloperId");
      }

      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "SideProjects");

         migrationBuilder.DropTable(
             name: "Experiences");

         migrationBuilder.DropTable(
             name: "Developers");
      }
   }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Migrations
{
    public partial class RenameToDraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(table: "Posts", name: "MarkedContent", newName: "Content");
            migrationBuilder.RenameTable("Posts", newName: "Drafts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Drafts", newName: "Posts");
            migrationBuilder.RenameColumn(table: "Posts", name: "Content", newName: "MarkedContent");
        }
    }
}

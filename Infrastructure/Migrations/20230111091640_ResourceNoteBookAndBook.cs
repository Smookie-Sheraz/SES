using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ResourceNoteBookAndBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceBook",
                table: "Setup_Book",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceBookPath",
                table: "Setup_Book",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResourceNoteBookId",
                table: "Setup_Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResourceNoteBook",
                columns: table => new
                {
                    ResourceNoteBookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteBookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceNoteBook", x => x.ResourceNoteBookId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Book_ResourceNoteBookId",
                table: "Setup_Book",
                column: "ResourceNoteBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_ResourceNoteBookId",
                table: "Setup_Book",
                column: "ResourceNoteBookId",
                principalTable: "ResourceNoteBook",
                principalColumn: "ResourceNoteBookId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_ResourceNoteBookId",
                table: "Setup_Book");

            migrationBuilder.DropTable(
                name: "ResourceNoteBook");

            migrationBuilder.DropIndex(
                name: "IX_Setup_Book_ResourceNoteBookId",
                table: "Setup_Book");

            migrationBuilder.DropColumn(
                name: "ResourceBook",
                table: "Setup_Book");

            migrationBuilder.DropColumn(
                name: "ResourceBookPath",
                table: "Setup_Book");

            migrationBuilder.DropColumn(
                name: "ResourceNoteBookId",
                table: "Setup_Book");
        }
    }
}

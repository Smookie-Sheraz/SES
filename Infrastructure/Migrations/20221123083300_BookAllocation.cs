using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class BookAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "SectionBook");

            migrationBuilder.CreateTable(
                name: "Setup_BookAllocation",
                columns: table => new
                {
                    BookAllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAllocation", x => x.BookAllocationId);
                    table.ForeignKey(
                        name: "FK_BookAllocation_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAllocation_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setup_BookAllocation_BookId",
                table: "Setup_BookAllocation",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_BookAllocation_SectionId",
                table: "Setup_BookAllocation",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setup_BookAllocation");

            migrationBuilder.CreateTable(
                name: "SectionBook",
                columns: table => new
                {
                    SectionBookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionBook", x => x.SectionBookId);
                    table.ForeignKey(
                        name: "FK_SectionBook_Setup_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SectionBook_Setup_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SectionBook_BookId",
                table: "SectionBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionBook_SectionId",
                table: "SectionBook",
                column: "SectionId",
                unique: true);
        }
    }
}

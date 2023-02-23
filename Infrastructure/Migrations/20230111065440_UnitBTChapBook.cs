using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UnitBTChapBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_BookId",
                table: "Chapter");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_BookId",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Chapter");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "Chapter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    UnitName = table.Column<string>(type: "varchar(200)", nullable: true),
                    StartPage = table.Column<int>(type: "int", nullable: true),
                    EndPage = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsAllocated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Unit_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_UnitId",
                table: "Chapter",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_BookId",
                table: "Unit",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_UnitId",
                table: "Chapter",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_UnitId",
                table: "Chapter");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_Chapter_UnitId",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Chapter");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Chapter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_BookId",
                table: "Chapter",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_BookId",
                table: "Chapter",
                column: "BookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

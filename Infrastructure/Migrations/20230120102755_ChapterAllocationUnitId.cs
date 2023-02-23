using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChapterAllocationUnitId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "ChapterAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChapterAllocation_UnitId",
                table: "ChapterAllocation",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation");

            migrationBuilder.DropIndex(
                name: "IX_ChapterAllocation_UnitId",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ChapterAllocation");
        }
    }
}

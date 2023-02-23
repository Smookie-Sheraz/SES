using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class GradeIdAddedInBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Setup_Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Book_GradeId",
                table: "Setup_Book",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_GradeId",
                table: "Setup_Book",
                column: "GradeId",
                principalTable: "Setup_Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_GradeId",
                table: "Setup_Book");

            migrationBuilder.DropIndex(
                name: "IX_Setup_Book_GradeId",
                table: "Setup_Book");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Setup_Book");
        }
    }
}

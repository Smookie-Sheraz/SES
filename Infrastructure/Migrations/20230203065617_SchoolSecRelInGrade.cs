using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SchoolSecRelInGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolSectionId",
                table: "Setup_Grade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Grade_SchoolSectionId",
                table: "Setup_Grade",
                column: "SchoolSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_SchoolSectionId",
                table: "Setup_Grade",
                column: "SchoolSectionId",
                principalTable: "Setup_SchoolSection",
                principalColumn: "SchoolSectionId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_SchoolSectionId",
                table: "Setup_Grade");

            migrationBuilder.DropIndex(
                name: "IX_Setup_Grade_SchoolSectionId",
                table: "Setup_Grade");

            migrationBuilder.DropColumn(
                name: "SchoolSectionId",
                table: "Setup_Grade");
        }
    }
}

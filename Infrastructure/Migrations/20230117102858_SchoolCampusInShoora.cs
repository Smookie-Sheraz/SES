using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SchoolCampusInShoora : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampdusId",
                table: "Shoora",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Shoora",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoora_CampdusId",
                table: "Shoora",
                column: "CampdusId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoora_SchoolId",
                table: "Shoora",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoora_CampusId",
                table: "Shoora",
                column: "CampdusId",
                principalTable: "Setup_Campus",
                principalColumn: "CampusId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoora_SchoolId",
                table: "Shoora",
                column: "SchoolId",
                principalTable: "Setup_School",
                principalColumn: "SchoolId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoora_CampusId",
                table: "Shoora");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoora_SchoolId",
                table: "Shoora");

            migrationBuilder.DropIndex(
                name: "IX_Shoora_CampdusId",
                table: "Shoora");

            migrationBuilder.DropIndex(
                name: "IX_Shoora_SchoolId",
                table: "Shoora");

            migrationBuilder.DropColumn(
                name: "CampdusId",
                table: "Shoora");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Shoora");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class PlanBookSubId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "AcademicPlanning",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "AcademicPlanning",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanning_BookId",
                table: "AcademicPlanning",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanning_SubjectId",
                table: "AcademicPlanning",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlannings_BookId",
                table: "AcademicPlanning",
                column: "BookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlannings_SubjectId",
                table: "AcademicPlanning",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlannings_BookId",
                table: "AcademicPlanning");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlannings_SubjectId",
                table: "AcademicPlanning");

            migrationBuilder.DropIndex(
                name: "IX_AcademicPlanning_BookId",
                table: "AcademicPlanning");

            migrationBuilder.DropIndex(
                name: "IX_AcademicPlanning_SubjectId",
                table: "AcademicPlanning");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "AcademicPlanning");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AcademicPlanning");
        }
    }
}

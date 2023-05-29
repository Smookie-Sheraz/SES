using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class PlanClassId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlanning_Employee_EmployeeId",
                table: "AcademicPlanning");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "AcademicPlanning",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanning_ClassId",
                table: "AcademicPlanning",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlannings_ClassId",
                table: "AcademicPlanning",
                column: "ClassId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlannings_EmployeeId",
                table: "AcademicPlanning",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlannings_ClassId",
                table: "AcademicPlanning");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlannings_EmployeeId",
                table: "AcademicPlanning");

            migrationBuilder.DropIndex(
                name: "IX_AcademicPlanning_ClassId",
                table: "AcademicPlanning");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "AcademicPlanning");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlanning_Employee_EmployeeId",
                table: "AcademicPlanning",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }
    }
}

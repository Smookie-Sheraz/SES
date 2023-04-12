using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class PlanApprovalsP2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssistantCoordinatorId",
                table: "Setup_SchoolSection",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "AcademicPlanning",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SchoolSection_AssistantCoordinatorId",
                table: "Setup_SchoolSection",
                column: "AssistantCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPlanning_EmployeeId",
                table: "AcademicPlanning",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicPlanning_Employee_EmployeeId",
                table: "AcademicPlanning",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSection_AssistantCoordinatorId",
                table: "Setup_SchoolSection",
                column: "AssistantCoordinatorId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicPlanning_Employee_EmployeeId",
                table: "AcademicPlanning");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSection_AssistantCoordinatorId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropIndex(
                name: "IX_Setup_SchoolSection_AssistantCoordinatorId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropIndex(
                name: "IX_AcademicPlanning_EmployeeId",
                table: "AcademicPlanning");

            migrationBuilder.DropColumn(
                name: "AssistantCoordinatorId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AcademicPlanning");
        }
    }
}

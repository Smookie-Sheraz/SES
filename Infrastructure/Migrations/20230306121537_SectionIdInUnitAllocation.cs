using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SectionIdInUnitAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setup_Grade_Employee_GradeManagerId",
                table: "Setup_Grade");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "UnitAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitAllocation_ClassId",
                table: "UnitAllocation",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_GradeManagerId",
                table: "Setup_Grade",
                column: "GradeManagerId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_SectionId",
                table: "UnitAllocation",
                column: "ClassId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_GradeManagerId",
                table: "Setup_Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_SectionId",
                table: "UnitAllocation");

            migrationBuilder.DropIndex(
                name: "IX_UnitAllocation_ClassId",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "UnitAllocation");

            migrationBuilder.AddForeignKey(
                name: "FK_Setup_Grade_Employee_GradeManagerId",
                table: "Setup_Grade",
                column: "GradeManagerId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }
    }
}

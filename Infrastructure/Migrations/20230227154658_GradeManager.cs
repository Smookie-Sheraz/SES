using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class GradeManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeManagerId",
                table: "Setup_Grade",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Grade_GradeManagerId",
                table: "Setup_Grade",
                column: "GradeManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setup_Grade_Employee_GradeManagerId",
                table: "Setup_Grade",
                column: "GradeManagerId",
                principalTable: "Employee",
                principalColumn: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setup_Grade_Employee_GradeManagerId",
                table: "Setup_Grade");

            migrationBuilder.DropIndex(
                name: "IX_Setup_Grade_GradeManagerId",
                table: "Setup_Grade");

            migrationBuilder.DropColumn(
                name: "GradeManagerId",
                table: "Setup_Grade");
        }
    }
}

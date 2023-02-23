using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class EmployeeAddRelationshipsAndSubAlloRelationsCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "SubjectAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "SubjectAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Setup_SchoolSection",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolSectionId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SchoolSection_SchoolId",
                table: "Setup_SchoolSection",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CampusId",
                table: "Employee",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SchoolId",
                table: "Employee",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SchoolSectionId",
                table: "Employee",
                column: "SchoolSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_CampusId",
                table: "Employee",
                column: "CampusId",
                principalTable: "Setup_Campus",
                principalColumn: "CampusId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_SchoolId",
                table: "Employee",
                column: "SchoolId",
                principalTable: "Setup_School",
                principalColumn: "SchoolId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_SchoolSectionId",
                table: "Employee",
                column: "SchoolSectionId",
                principalTable: "Setup_SchoolSection",
                principalColumn: "SchoolSectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSection_SchoolId",
                table: "Setup_SchoolSection",
                column: "SchoolId",
                principalTable: "Setup_School",
                principalColumn: "SchoolId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_CampusId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_SchoolId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_SchoolSectionId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSection_SchoolId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation");

            migrationBuilder.DropIndex(
                name: "IX_Setup_SchoolSection_SchoolId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropIndex(
                name: "IX_Employee_CampusId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SchoolId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SchoolSectionId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Setup_SchoolSection");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SchoolSectionId",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "SubjectAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "SubjectAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId");
        }
    }
}

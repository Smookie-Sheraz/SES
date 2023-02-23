using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class HeadShooraEmployeeSubDep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubDepartment_HeadId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropIndex(
                name: "IX_Setup_SubDepartment_HeadId",
                table: "Setup_SubDepartment");

            migrationBuilder.AlterColumn<int>(
                name: "SubDepartmentId",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DesignationId",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employee",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubDepartmentHeadSubDepartmentId",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SubDepartment_HeadId",
                table: "Setup_SubDepartment",
                column: "HeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SubDepartmentHeadSubDepartmentId",
                table: "Employee",
                column: "SubDepartmentHeadSubDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Setup_SubDepartment_SubDepartmentHeadSubDepartmentId",
                table: "Employee",
                column: "SubDepartmentHeadSubDepartmentId",
                principalTable: "Setup_SubDepartment",
                principalColumn: "SubDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDepartment_HeadId",
                table: "Setup_SubDepartment",
                column: "HeadId",
                principalTable: "Setup_Head",
                principalColumn: "HeadId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Setup_SubDepartment_SubDepartmentHeadSubDepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_SubDepartment_HeadId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropIndex(
                name: "IX_Setup_SubDepartment_HeadId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropIndex(
                name: "IX_Employee_SubDepartmentHeadSubDepartmentId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SubDepartmentHeadSubDepartmentId",
                table: "Employee");

            migrationBuilder.AlterColumn<int>(
                name: "SubDepartmentId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DesignationId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SubDepartment_HeadId",
                table: "Setup_SubDepartment",
                column: "HeadId",
                unique: true,
                filter: "[HeadId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SubDepartment_HeadId",
                table: "Setup_SubDepartment",
                column: "HeadId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

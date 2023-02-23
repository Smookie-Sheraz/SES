using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setup_Designation",
                columns: table => new
                {
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.DesignationId);
                });

            migrationBuilder.CreateTable(
                name: "Setup_Gender",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WGender = table.Column<string>(type: "varchar(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(type: "varchar(100)", nullable: true),
                    LName = table.Column<string>(type: "varchar(100)", nullable: true),
                    FatherName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Email = table.Column<string>(type: "varchar(200)", nullable: true),
                    UserName = table.Column<string>(type: "varchar(100)", nullable: true),
                    Password = table.Column<string>(type: "varchar(100)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Shoora",
                columns: table => new
                {
                    ShooraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    LName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    FatherName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    SpouseName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "varchar(100)", nullable: true),
                    Mobile = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    CNICNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    CNICIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    CNICExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "varchar(500)", nullable: true),
                    Address = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoora", x => x.ShooraId);
                    table.ForeignKey(
                        name: "FK_Shoora_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Setup_Gender",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Setup_Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    DepartmentHeadId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    ShortDescripiton = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Department_ShooraId",
                        column: x => x.DepartmentHeadId,
                        principalTable: "Shoora",
                        principalColumn: "ShooraId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    FName = table.Column<string>(type: "varchar(100)", nullable: true),
                    LName = table.Column<string>(type: "varchar(100)", nullable: true),
                    FatherName = table.Column<string>(type: "varchar(100)", nullable: true),
                    SpouseName = table.Column<string>(type: "varchar(100)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "varchar(100)", nullable: true),
                    Mobile = table.Column<string>(type: "varchar(11)", nullable: true),
                    DOB = table.Column<DateTime>(type: "date", nullable: true),
                    CNICNo = table.Column<string>(type: "varchar(15)", nullable: true),
                    CNICIssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    CNICExpiryDate = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "varchar(500)", nullable: true),
                    Address = table.Column<string>(type: "varchar(1000)", nullable: true),
                    FieldOfSpecialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProbationPeriod = table.Column<int>(type: "int", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: true),
                    StartofPeriodDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndofPeriodDate = table.Column<DateTime>(type: "date", nullable: true),
                    StartofProbationDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndofProbationDate = table.Column<DateTime>(type: "date", nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeType = table.Column<int>(type: "int", nullable: true),
                    SubDepartmentId = table.Column<int>(type: "int", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Setup_Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employee_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Setup_Designation",
                        principalColumn: "DesignationId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employee_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Setup_Gender",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Setup_Head",
                columns: table => new
                {
                    HeadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShooraId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Head", x => x.HeadId);
                    table.ForeignKey(
                        name: "FK_Head_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Head_ShooraId",
                        column: x => x.ShooraId,
                        principalTable: "Shoora",
                        principalColumn: "ShooraId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Setup_SubDepartment",
                columns: table => new
                {
                    SubDepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    MainDepartmentId = table.Column<int>(type: "int", nullable: false),
                    HeadId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDepartment", x => x.SubDepartmentId);
                    table.ForeignKey(
                        name: "FK_Subdepartment_DepartmentId",
                        column: x => x.MainDepartmentId,
                        principalTable: "Setup_Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubDepartment_HeadId",
                        column: x => x.HeadId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DesignationId",
                table: "Employee",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_GenderId",
                table: "Employee",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_SubDepartmentId",
                table: "Employee",
                column: "SubDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Department_DepartmentHeadId",
                table: "Setup_Department",
                column: "DepartmentHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Head_EmployeeId",
                table: "Setup_Head",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Head_ShooraId",
                table: "Setup_Head",
                column: "ShooraId");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SubDepartment_HeadId",
                table: "Setup_SubDepartment",
                column: "HeadId",
                unique: true,
                filter: "[HeadId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Setup_SubDepartment_MainDepartmentId",
                table: "Setup_SubDepartment",
                column: "MainDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoora_GenderId",
                table: "Shoora",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_SubDepartmentId",
                table: "Employee",
                column: "SubDepartmentId",
                principalTable: "Setup_SubDepartment",
                principalColumn: "SubDepartmentId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdepartment_DepartmentId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_DesignationId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_GenderId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_SubDepartmentId",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "Setup_Head");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Setup_Department");

            migrationBuilder.DropTable(
                name: "Shoora");

            migrationBuilder.DropTable(
                name: "Setup_Designation");

            migrationBuilder.DropTable(
                name: "Setup_Gender");

            migrationBuilder.DropTable(
                name: "Setup_SubDepartment");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}

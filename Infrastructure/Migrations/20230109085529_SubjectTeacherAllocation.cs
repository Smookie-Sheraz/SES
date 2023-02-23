using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SubjectTeacherAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectTeacherAllocation",
                columns: table => new
                {
                    SubjectTeacherAllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<string>(type: "varchar(30)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeacherAllocation", x => x.SubjectTeacherAllocationId);
                    table.ForeignKey(
                        name: "FK_SubjectTeacherAllocation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SubjectTeacherAllocation_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SubjectTeacherAllocation_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Setup_Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacherAllocation_EmployeeId",
                table: "SubjectTeacherAllocation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacherAllocation_SectionId",
                table: "SubjectTeacherAllocation",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeacherAllocation_SubjectId",
                table: "SubjectTeacherAllocation",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectTeacherAllocation");
        }
    }
}

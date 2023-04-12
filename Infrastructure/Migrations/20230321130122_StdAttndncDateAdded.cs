using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class StdAttndncDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentAttendance",
                columns: table => new
                {
                    StudentAttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateOrOnTime = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "On Time"),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendance", x => x.StudentAttendanceId);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassId",
                table: "StudentAttendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_StudentId",
                table: "StudentAttendance",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAttendance");
        }
    }
}

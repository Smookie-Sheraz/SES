using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TestandDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    TestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMarks = table.Column<int>(type: "int", nullable: true),
                    ObtainedMarks = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestId);
                    table.ForeignKey(
                        name: "FK_Test_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Test_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Test_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Setup_Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Diary",
                columns: table => new
                {
                    DiaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diary", x => x.DiaryId);
                    table.ForeignKey(
                        name: "FK_Diary_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Diary_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Setup_Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Diary_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diary_ClassId",
                table: "Diary",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Diary_SubjectId",
                table: "Diary",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Diary_TestId",
                table: "Diary",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_BookId",
                table: "Test",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_ClassId",
                table: "Test",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_SubjectId",
                table: "Test",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diary");

            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}

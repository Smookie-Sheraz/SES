using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TeachSubAlloSubIdToBookId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacherAllocation_SubjectId",
                table: "SubjectTeacherAllocation");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "SubjectTeacherAllocation",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectTeacherAllocation_SubjectId",
                table: "SubjectTeacherAllocation",
                newName: "IX_SubjectTeacherAllocation_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacherAllocation_BookId",
                table: "SubjectTeacherAllocation",
                column: "BookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacherAllocation_BookId",
                table: "SubjectTeacherAllocation");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "SubjectTeacherAllocation",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_SubjectTeacherAllocation_BookId",
                table: "SubjectTeacherAllocation",
                newName: "IX_SubjectTeacherAllocation_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacherAllocation_SubjectId",
                table: "SubjectTeacherAllocation",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class FKeysSettedNullsIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_ChapterId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_SubjectId",
                table: "Setup_Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAllocation_BookId",
                table: "Setup_BookAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAllocation_SectionId",
                table: "Setup_BookAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_GradeId",
                table: "Setup_Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdepartment_DepartmentId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Term_YearId",
                table: "Setup_Term");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopic_TopicId",
                table: "SubTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_SubTopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TopicId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_UnitId",
                table: "UnitAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "UnitAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "UnitAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UnitAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "TopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "TopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TopicAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubTopicId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubTopicAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "SubTopic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubjectTeacherAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SubjectAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Shoora",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_Year",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "Setup_Term",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_Term",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainDepartmentId",
                table: "Setup_SubDepartment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_SubDepartment",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "Setup_Section",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_SchoolSection",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_School",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_Gender",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_Campus",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Setup_BookAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Setup_BookAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Setup_BookAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Setup_Book",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ResourceNoteBook",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employee",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImageUrl",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChapterAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_ChapterId",
                table: "ChapterAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId",
                principalTable: "Setup_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_SubjectId",
                table: "Setup_Book",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAllocation_BookId",
                table: "Setup_BookAllocation",
                column: "BookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAllocation_SectionId",
                table: "Setup_BookAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_GradeId",
                table: "Setup_Section",
                column: "GradeId",
                principalTable: "Setup_Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Subdepartment_DepartmentId",
                table: "Setup_SubDepartment",
                column: "MainDepartmentId",
                principalTable: "Setup_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Term_YearId",
                table: "Setup_Term",
                column: "YearId",
                principalTable: "Setup_Year",
                principalColumn: "YearId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopic_TopicId",
                table: "SubTopic",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_SubTopicId",
                table: "SubTopicAllocation",
                column: "SubTopicId",
                principalTable: "SubTopic",
                principalColumn: "SubTopicId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TopicId",
                table: "TopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_UnitId",
                table: "UnitAllocation",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_ChapterId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_SubjectId",
                table: "Setup_Book");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAllocation_BookId",
                table: "Setup_BookAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAllocation_SectionId",
                table: "Setup_BookAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_GradeId",
                table: "Setup_Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Subdepartment_DepartmentId",
                table: "Setup_SubDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Term_YearId",
                table: "Setup_Term");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopic_TopicId",
                table: "SubTopic");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_SubTopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TopicId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_UnitId",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubjectTeacherAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SubjectAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Shoora");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_Year");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_Term");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_SubDepartment");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_SchoolSection");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_School");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_Gender");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_Campus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Setup_BookAllocation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ResourceNoteBook");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserImageUrl",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChapterAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "UnitAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "UnitAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "TopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "TopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "TopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubTopicId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "SubTopic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "Setup_Term",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainDepartmentId",
                table: "Setup_SubDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "Setup_Section",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "Setup_BookAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Setup_BookAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Setup_Book",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "ChapterAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TermId",
                table: "ChapterAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "ChapterAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_ChapterId",
                table: "ChapterAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_UnitId",
                table: "ChapterAllocation",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId",
                principalTable: "Setup_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_SubjectId",
                table: "Setup_Book",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAllocation_BookId",
                table: "Setup_BookAllocation",
                column: "BookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAllocation_SectionId",
                table: "Setup_BookAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_GradeId",
                table: "Setup_Section",
                column: "GradeId",
                principalTable: "Setup_Grade",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subdepartment_DepartmentId",
                table: "Setup_SubDepartment",
                column: "MainDepartmentId",
                principalTable: "Setup_Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Term_YearId",
                table: "Setup_Term",
                column: "YearId",
                principalTable: "Setup_Year",
                principalColumn: "YearId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopic_TopicId",
                table: "SubTopic",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_SubTopicId",
                table: "SubTopicAllocation",
                column: "SubTopicId",
                principalTable: "SubTopic",
                principalColumn: "SubTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TopicId",
                table: "TopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_UnitId",
                table: "UnitAllocation",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

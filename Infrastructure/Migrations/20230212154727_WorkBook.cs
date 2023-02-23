using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class WorkBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkBookEndPage",
                table: "UnitAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookId",
                table: "UnitAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookStartPage",
                table: "UnitAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookEndPage",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookId",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookStartPage",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookEndPage",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookStartPage",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkBook",
                table: "Setup_Book",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookEndPage",
                table: "ChapterAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkBookStartPage",
                table: "ChapterAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnitAllocation_WorkBookId",
                table: "UnitAllocation",
                column: "WorkBookId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicAllocation_WorkBookId",
                table: "TopicAllocation",
                column: "WorkBookId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_WorkBookId",
                table: "SubTopicAllocation",
                column: "WorkBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterAllocation_WorkBookId",
                table: "ChapterAllocation",
                column: "WorkBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_WorkBookId",
                table: "ChapterAllocation",
                column: "WorkBookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_WorkBookId",
                table: "SubTopicAllocation",
                column: "WorkBookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_WorkBookId",
                table: "TopicAllocation",
                column: "WorkBookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_WorkBookId",
                table: "UnitAllocation",
                column: "WorkBookId",
                principalTable: "Setup_Book",
                principalColumn: "BookId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_WorkBookId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_WorkBookId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_WorkBookId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_WorkBookId",
                table: "UnitAllocation");

            migrationBuilder.DropIndex(
                name: "IX_UnitAllocation_WorkBookId",
                table: "UnitAllocation");

            migrationBuilder.DropIndex(
                name: "IX_TopicAllocation_WorkBookId",
                table: "TopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_SubTopicAllocation_WorkBookId",
                table: "SubTopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_ChapterAllocation_WorkBookId",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookEndPage",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookId",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookStartPage",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookEndPage",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookId",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookStartPage",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookEndPage",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookId",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookStartPage",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "IsWorkBook",
                table: "Setup_Book");

            migrationBuilder.DropColumn(
                name: "WorkBookEndPage",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookId",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "WorkBookStartPage",
                table: "ChapterAllocation");
        }
    }
}

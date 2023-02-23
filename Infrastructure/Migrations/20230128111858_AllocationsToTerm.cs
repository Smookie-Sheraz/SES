using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AllocationsToTerm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_MonthId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_MonthId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_MonthId",
                table: "UnitAllocation");

            migrationBuilder.RenameColumn(
                name: "MonthId",
                table: "UnitAllocation",
                newName: "TermId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitAllocation_MonthId",
                table: "UnitAllocation",
                newName: "IX_UnitAllocation_TermId");

            migrationBuilder.RenameColumn(
                name: "MonthId",
                table: "TopicAllocation",
                newName: "TermId");

            migrationBuilder.RenameIndex(
                name: "IX_TopicAllocation_MonthId",
                table: "TopicAllocation",
                newName: "IX_TopicAllocation_TermId");

            migrationBuilder.RenameColumn(
                name: "MonthId",
                table: "SubTopicAllocation",
                newName: "TermId");

            migrationBuilder.RenameIndex(
                name: "IX_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation",
                newName: "IX_SubTopicAllocation_TermId");

            migrationBuilder.RenameColumn(
                name: "MonthId",
                table: "ChapterAllocation",
                newName: "TermId");

            migrationBuilder.RenameIndex(
                name: "IX_ChapterAllocation_MonthId",
                table: "ChapterAllocation",
                newName: "IX_ChapterAllocation_TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation",
                column: "TermId",
                principalTable: "Setup_Term",
                principalColumn: "TermId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_TermId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TermId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TermId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_TermId",
                table: "UnitAllocation");

            migrationBuilder.RenameColumn(
                name: "TermId",
                table: "UnitAllocation",
                newName: "MonthId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitAllocation_TermId",
                table: "UnitAllocation",
                newName: "IX_UnitAllocation_MonthId");

            migrationBuilder.RenameColumn(
                name: "TermId",
                table: "TopicAllocation",
                newName: "MonthId");

            migrationBuilder.RenameIndex(
                name: "IX_TopicAllocation_TermId",
                table: "TopicAllocation",
                newName: "IX_TopicAllocation_MonthId");

            migrationBuilder.RenameColumn(
                name: "TermId",
                table: "SubTopicAllocation",
                newName: "MonthId");

            migrationBuilder.RenameIndex(
                name: "IX_SubTopicAllocation_TermId",
                table: "SubTopicAllocation",
                newName: "IX_SubTopicAllocation_MonthId");

            migrationBuilder.RenameColumn(
                name: "TermId",
                table: "ChapterAllocation",
                newName: "MonthId");

            migrationBuilder.RenameIndex(
                name: "IX_ChapterAllocation_TermId",
                table: "ChapterAllocation",
                newName: "IX_ChapterAllocation_MonthId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_MonthId",
                table: "ChapterAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_MonthId",
                table: "TopicAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_MonthId",
                table: "UnitAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

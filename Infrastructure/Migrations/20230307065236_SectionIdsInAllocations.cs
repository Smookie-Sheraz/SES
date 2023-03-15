using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SectionIdsInAllocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "UnitAllocation",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitAllocation_ClassId",
                table: "UnitAllocation",
                newName: "IX_UnitAllocation_SectionId");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicAllocation_SectionId",
                table: "TopicAllocation",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_SectionId",
                table: "SubTopicAllocation",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterAllocation_SectionId",
                table: "ChapterAllocation",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_SectionId",
                table: "ChapterAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_SectionId",
                table: "SubTopicAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_SectionId",
                table: "TopicAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_SectionId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_SectionId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_SectionId",
                table: "TopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_TopicAllocation_SectionId",
                table: "TopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_SubTopicAllocation_SectionId",
                table: "SubTopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_ChapterAllocation_SectionId",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "ChapterAllocation");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "UnitAllocation",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_UnitAllocation_SectionId",
                table: "UnitAllocation",
                newName: "IX_UnitAllocation_ClassId");
        }
    }
}

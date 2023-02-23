using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TeachMethodChapSLO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_ChapterId",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "Topic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TeachingMethodDesc",
                table: "Topic",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeachingMethodologyId",
                table: "Topic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLO",
                table: "Chapter",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeachingMethodology",
                columns: table => new
                {
                    TeachingMethodologyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TMethodologyName = table.Column<string>(type: "varchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "varchar(30)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachingMethodology", x => x.TeachingMethodologyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Topic_TeachingMethodologyId",
                table: "Topic",
                column: "TeachingMethodologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_ChapterId",
                table: "Topic",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_TeachingMethodologyId",
                table: "Topic",
                column: "TeachingMethodologyId",
                principalTable: "TeachingMethodology",
                principalColumn: "TeachingMethodologyId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_ChapterId",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation");

            migrationBuilder.DropTable(
                name: "TeachingMethodology");

            migrationBuilder.DropIndex(
                name: "IX_Topic_TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TeachingMethodDesc",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "SLO",
                table: "Chapter");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterId",
                table: "Topic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_ChapterId",
                table: "Topic",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_ChapterId",
                table: "TopicAllocation",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId");
        }
    }
}

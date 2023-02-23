using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TopicAlloTMethodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TeachingMethodDesc",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TeachingMethodologyId",
                table: "Topic");

            migrationBuilder.AddColumn<string>(
                name: "TMethodDesc",
                table: "TopicAllocation",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeachingMethodologyId",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicAllocation_TeachingMethodologyId",
                table: "TopicAllocation",
                column: "TeachingMethodologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_TeachingMethodologyId",
                table: "TopicAllocation",
                column: "TeachingMethodologyId",
                principalTable: "TeachingMethodology",
                principalColumn: "TeachingMethodologyId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_TeachingMethodologyId",
                table: "TopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_TopicAllocation_TeachingMethodologyId",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "TMethodDesc",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "TeachingMethodologyId",
                table: "TopicAllocation");

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

            migrationBuilder.CreateIndex(
                name: "IX_Topic_TeachingMethodologyId",
                table: "Topic",
                column: "TeachingMethodologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_TeachingMethodologyId",
                table: "Topic",
                column: "TeachingMethodologyId",
                principalTable: "TeachingMethodology",
                principalColumn: "TeachingMethodologyId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}

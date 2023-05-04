using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AreSaturdaysOffPlanning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AreSaturdaysOff",
                table: "UnitAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreSaturdaysOff",
                table: "TopicAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreSaturdaysOff",
                table: "SubTopicAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreSaturdaysOff",
                table: "ChapterAllocation",
                type: "bit",
                nullable: true,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreSaturdaysOff",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "AreSaturdaysOff",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "AreSaturdaysOff",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "AreSaturdaysOff",
                table: "ChapterAllocation");
        }
    }
}

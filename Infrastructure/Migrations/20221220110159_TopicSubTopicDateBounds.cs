using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class TopicSubTopicDateBounds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChapterEndDate",
                table: "TopicAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ChapterStartDate",
                table: "TopicAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TopicEndDate",
                table: "SubTopicAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TopicStartDate",
                table: "SubTopicAllocation",
                type: "date",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChapterEndDate",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "ChapterStartDate",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "TopicEndDate",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "TopicStartDate",
                table: "SubTopicAllocation");
        }
    }
}

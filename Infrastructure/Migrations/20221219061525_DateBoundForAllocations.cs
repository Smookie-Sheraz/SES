using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class DateBoundForAllocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TopicDeliveryDate",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "SubTopic");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "SubTopic");

            migrationBuilder.RenameColumn(
                name: "TopicDeliveryDate",
                table: "TopicAllocation",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TopicAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ChapterAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ChapterAllocation",
                type: "date",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ChapterAllocation");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "TopicAllocation",
                newName: "TopicDeliveryDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Topic",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Topic",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TopicDeliveryDate",
                table: "SubTopicAllocation",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "SubTopic",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "SubTopic",
                type: "date",
                nullable: true);
        }
    }
}

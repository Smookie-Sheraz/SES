using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SubTopicAllocationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubTopicAllocation",
                columns: table => new
                {
                    SubTopicAllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    SubTopicId = table.Column<int>(type: "int", nullable: false),
                    IsAllocated = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TopicDeliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedDate = table.Column<string>(type: "varchar(30)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTopicAllocation", x => x.SubTopicAllocationId);
                    table.ForeignKey(
                        name: "FK_SubTopicAllocation_MonthId",
                        column: x => x.MonthId,
                        principalTable: "Setup_Month",
                        principalColumn: "MonthId");
                    table.ForeignKey(
                        name: "FK_SubTopicAllocation_SubTopicId",
                        column: x => x.SubTopicId,
                        principalTable: "SubTopic",
                        principalColumn: "SubTopicId");
                    table.ForeignKey(
                        name: "FK_SubTopicAllocation_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topic",
                        principalColumn: "TopicId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation",
                column: "MonthId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_SubTopicId",
                table: "SubTopicAllocation",
                column: "SubTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_TopicId",
                table: "SubTopicAllocation",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTopicAllocation");
        }
    }
}

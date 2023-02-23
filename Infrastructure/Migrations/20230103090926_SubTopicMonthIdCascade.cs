using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SubTopicMonthIdCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation");

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_MonthId",
                table: "SubTopicAllocation",
                column: "MonthId",
                principalTable: "Setup_Month",
                principalColumn: "MonthId");
        }
    }
}

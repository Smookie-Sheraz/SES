using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AcademicPlanningsforApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "UnitAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "TopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "SubTopicAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "ChapterAllocation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicPlanning",
                columns: table => new
                {
                    AcademicPlanningsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlannedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicPlanning", x => x.AcademicPlanningsId);
                });

            migrationBuilder.CreateTable(
                name: "PlanApproval",
                columns: table => new
                {
                    PlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovingPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovingPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanApproval", x => x.PlanApprovalId);
                    table.ForeignKey(
                        name: "FK_PlanApproval_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlanApproval_PlanId",
                        column: x => x.PlanId,
                        principalTable: "AcademicPlanning",
                        principalColumn: "AcademicPlanningsId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitAllocation_PlanId",
                table: "UnitAllocation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicAllocation_PlanId",
                table: "TopicAllocation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTopicAllocation_PlanId",
                table: "SubTopicAllocation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterAllocation_PlanId",
                table: "ChapterAllocation",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanApproval_BookId",
                table: "PlanApproval",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanApproval_PlanId",
                table: "PlanApproval",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChapterAllocation_PlanId",
                table: "ChapterAllocation",
                column: "PlanId",
                principalTable: "AcademicPlanning",
                principalColumn: "AcademicPlanningsId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTopicAllocation_PlanId",
                table: "SubTopicAllocation",
                column: "PlanId",
                principalTable: "AcademicPlanning",
                principalColumn: "AcademicPlanningsId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicAllocation_PlanId",
                table: "TopicAllocation",
                column: "PlanId",
                principalTable: "AcademicPlanning",
                principalColumn: "AcademicPlanningsId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAllocation_PlanId",
                table: "UnitAllocation",
                column: "PlanId",
                principalTable: "AcademicPlanning",
                principalColumn: "AcademicPlanningsId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChapterAllocation_PlanId",
                table: "ChapterAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTopicAllocation_PlanId",
                table: "SubTopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicAllocation_PlanId",
                table: "TopicAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAllocation_PlanId",
                table: "UnitAllocation");

            migrationBuilder.DropTable(
                name: "PlanApproval");

            migrationBuilder.DropTable(
                name: "AcademicPlanning");

            migrationBuilder.DropIndex(
                name: "IX_UnitAllocation_PlanId",
                table: "UnitAllocation");

            migrationBuilder.DropIndex(
                name: "IX_TopicAllocation_PlanId",
                table: "TopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_SubTopicAllocation_PlanId",
                table: "SubTopicAllocation");

            migrationBuilder.DropIndex(
                name: "IX_ChapterAllocation_PlanId",
                table: "ChapterAllocation");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "UnitAllocation");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "TopicAllocation");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "SubTopicAllocation");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "ChapterAllocation");
        }
    }
}

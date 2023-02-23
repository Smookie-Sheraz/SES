using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class MonthRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setup_Month");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setup_Month",
                columns: table => new
                {
                    MonthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    AssesmentDays = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Event = table.Column<string>(type: "varchar(500)", nullable: true),
                    EventDate = table.Column<DateTime>(type: "date", nullable: true),
                    Holidays = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    TotalSatSundays = table.Column<int>(type: "int", nullable: true),
                    TotalSchoolDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Month", x => x.MonthId);
                    table.ForeignKey(
                        name: "FK_Month_TermId",
                        column: x => x.TermId,
                        principalTable: "Setup_Term",
                        principalColumn: "TermId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Month_TermId",
                table: "Setup_Month",
                column: "TermId");
        }
    }
}

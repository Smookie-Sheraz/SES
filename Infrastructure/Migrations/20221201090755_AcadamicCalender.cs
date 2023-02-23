using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AcadamicCalender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Setup_BookAllocation");

            migrationBuilder.AddColumn<bool>(
                name: "preExist",
                table: "Setup_BookAllocation",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "Setup_Year",
                columns: table => new
                {
                    YearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    TotalSatSundays = table.Column<int>(type: "int", nullable: true),
                    Holidays = table.Column<int>(type: "int", nullable: true),
                    TotalSchoolDays = table.Column<int>(type: "int", nullable: true),
                    TotalAssesWiseSchoolDays = table.Column<int>(type: "int", nullable: true),
                    IsLeapYear = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Year", x => x.YearId);
                });

            migrationBuilder.CreateTable(
                name: "Setup_Term",
                columns: table => new
                {
                    TermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    TotalSchoolDays = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.TermId);
                    table.ForeignKey(
                        name: "FK_Term_YearId",
                        column: x => x.YearId,
                        principalTable: "Setup_Year",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Setup_Month",
                columns: table => new
                {
                    MonthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    TotalSatSundays = table.Column<int>(type: "int", nullable: true),
                    Event = table.Column<string>(type: "varchar(500)", nullable: true),
                    EventDate = table.Column<DateTime>(type: "date", nullable: true),
                    Holidays = table.Column<int>(type: "int", nullable: true),
                    TotalSchoolDays = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(30)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<string>(type: "varchar(30)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Term_YearId",
                table: "Setup_Term",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setup_Month");

            migrationBuilder.DropTable(
                name: "Setup_Term");

            migrationBuilder.DropTable(
                name: "Setup_Year");

            migrationBuilder.DropColumn(
                name: "preExist",
                table: "Setup_BookAllocation");

            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Setup_BookAllocation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

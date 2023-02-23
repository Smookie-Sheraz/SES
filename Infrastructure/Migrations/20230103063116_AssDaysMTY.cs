using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AssDaysMTY : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssesmentDays",
                table: "Setup_Year",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssesmentDays",
                table: "Setup_Term",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssesmentDays",
                table: "Setup_Month",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssesmentDays",
                table: "Setup_Year");

            migrationBuilder.DropColumn(
                name: "AssesmentDays",
                table: "Setup_Term");

            migrationBuilder.DropColumn(
                name: "AssesmentDays",
                table: "Setup_Month");
        }
    }
}

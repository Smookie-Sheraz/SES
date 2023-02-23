using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateTermModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Holidays",
                table: "Setup_Term",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalSatSun",
                table: "Setup_Term",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Holidays",
                table: "Setup_Term");

            migrationBuilder.DropColumn(
                name: "TotalSatSun",
                table: "Setup_Term");
        }
    }
}

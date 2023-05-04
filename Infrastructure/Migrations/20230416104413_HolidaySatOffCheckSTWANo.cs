using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class HolidaySatOffCheckSTWANo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhastAppNo",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AreSaturdaysOff",
                table: "Setup_Term",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSchoolOff",
                table: "Holidays",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhastAppNo",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "AreSaturdaysOff",
                table: "Setup_Term");

            migrationBuilder.DropColumn(
                name: "IsSchoolOff",
                table: "Holidays");
        }
    }
}

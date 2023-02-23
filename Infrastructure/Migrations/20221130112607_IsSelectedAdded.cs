using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class IsSelectedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}

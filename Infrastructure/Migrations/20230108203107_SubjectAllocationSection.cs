using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SubjectAllocationSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectAllocation",
                columns: table => new
                {
                    SubjectAllocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    preExist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAllocation", x => x.SubjectAllocationId);
                    table.ForeignKey(
                        name: "FK_SubjectAllocation_Setup_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId");
                    table.ForeignKey(
                        name: "FK_SubjectAllocation_Setup_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Setup_Subject",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAllocation_SectionId",
                table: "SubjectAllocation",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAllocation_SubjectId",
                table: "SubjectAllocation",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectAllocation");
        }
    }
}

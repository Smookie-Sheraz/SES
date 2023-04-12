using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class LetterNotice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolLatter",
                columns: table => new
                {
                    SchoolLatterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssuingACId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    SendingDate = table.Column<DateTime>(type: "date", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Closing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolLatter", x => x.SchoolLatterId);
                    table.ForeignKey(
                        name: "FK_SchoolLetter_IssuingACId",
                        column: x => x.IssuingACId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SchoolLetter_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parent",
                        principalColumn: "ParentId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SchoolLetter_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SchoolNotice",
                columns: table => new
                {
                    SchoolNoticeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssuingACId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: true),
                    BroadcastDate = table.Column<DateTime>(type: "date", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolNotice", x => x.SchoolNoticeId);
                    table.ForeignKey(
                        name: "FK_SchoolNotice_IssuingACId",
                        column: x => x.IssuingACId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLatter_IssuingACId",
                table: "SchoolLatter",
                column: "IssuingACId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLatter_ParentId",
                table: "SchoolLatter",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLatter_StudentId",
                table: "SchoolLatter",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolNotice_IssuingACId",
                table: "SchoolNotice",
                column: "IssuingACId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolLatter");

            migrationBuilder.DropTable(
                name: "SchoolNotice");
        }
    }
}

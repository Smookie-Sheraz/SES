using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ApprovalandCTId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassTeacherId",
                table: "Setup_Section",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "STPlanApproval",
                columns: table => new
                {
                    STPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    YearId = table.Column<int>(type: "int", nullable: true),
                    TermId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STPlanApproval", x => x.STPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_STPlanApproval_BookId",
                        column: x => x.BookId,
                        principalTable: "Setup_Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_STPlanApproval_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_STPlanApproval_Setup_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Setup_Section",
                        principalColumn: "SectionId");
                    table.ForeignKey(
                        name: "FK_STPlanApproval_TermId",
                        column: x => x.TermId,
                        principalTable: "Setup_Term",
                        principalColumn: "TermId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_STPlanApproval_YearId",
                        column: x => x.YearId,
                        principalTable: "Setup_Year",
                        principalColumn: "YearId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ACPlanApproval",
                columns: table => new
                {
                    ACPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACPlanApproval", x => x.ACPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_ACPlanApproval_STPlanId",
                        column: x => x.PlanId,
                        principalTable: "STPlanApproval",
                        principalColumn: "STPlanApprovalId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CTPlanApproval",
                columns: table => new
                {
                    CTPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CTPlanApproval", x => x.CTPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_CTPlanApproval_CTPlanId",
                        column: x => x.PlanId,
                        principalTable: "STPlanApproval",
                        principalColumn: "STPlanApprovalId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DAPlanApproval",
                columns: table => new
                {
                    DAPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DAPlanApproval", x => x.DAPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_DAPlanApproval_STPlanId",
                        column: x => x.PlanId,
                        principalTable: "STPlanApproval",
                        principalColumn: "STPlanApprovalId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DCPlanApproval",
                columns: table => new
                {
                    DCPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCPlanApproval", x => x.DCPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_DCPlanApproval_STPlanId",
                        column: x => x.PlanId,
                        principalTable: "STPlanApproval",
                        principalColumn: "STPlanApprovalId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GMPlanApproval",
                columns: table => new
                {
                    GMPlanApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GMApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    ACApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DCApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DAApproval = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    DeletedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GMPlanApproval", x => x.GMPlanApprovalId);
                    table.ForeignKey(
                        name: "FK_GMPlanApproval_STPlanId",
                        column: x => x.PlanId,
                        principalTable: "STPlanApproval",
                        principalColumn: "STPlanApprovalId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Setup_Section_ClassTeacherId",
                table: "Setup_Section",
                column: "ClassTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ACPlanApproval_PlanId",
                table: "ACPlanApproval",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_CTPlanApproval_PlanId",
                table: "CTPlanApproval",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DAPlanApproval_PlanId",
                table: "DAPlanApproval",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DCPlanApproval_PlanId",
                table: "DCPlanApproval",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_GMPlanApproval_PlanId",
                table: "GMPlanApproval",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_STPlanApproval_BookId",
                table: "STPlanApproval",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_STPlanApproval_EmployeeId",
                table: "STPlanApproval",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_STPlanApproval_SectionId",
                table: "STPlanApproval",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_STPlanApproval_TermId",
                table: "STPlanApproval",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_STPlanApproval_YearId",
                table: "STPlanApproval",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_ClassTeacherId",
                table: "Setup_Section",
                column: "ClassTeacherId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_ClassTeacherId",
                table: "Setup_Section");

            migrationBuilder.DropTable(
                name: "ACPlanApproval");

            migrationBuilder.DropTable(
                name: "CTPlanApproval");

            migrationBuilder.DropTable(
                name: "DAPlanApproval");

            migrationBuilder.DropTable(
                name: "DCPlanApproval");

            migrationBuilder.DropTable(
                name: "GMPlanApproval");

            migrationBuilder.DropTable(
                name: "STPlanApproval");

            migrationBuilder.DropIndex(
                name: "IX_Setup_Section_ClassTeacherId",
                table: "Setup_Section");

            migrationBuilder.DropColumn(
                name: "ClassTeacherId",
                table: "Setup_Section");
        }
    }
}

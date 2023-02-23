using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class subAlloPreExitRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_Setup_Section_SectionId",
                table: "SubjectAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_Setup_Subject_SubjectId",
                table: "SubjectAllocation");

            migrationBuilder.DropColumn(
                name: "preExist",
                table: "SubjectAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "SubjectAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "SubjectAllocation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubjectAllocation",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedOn",
                table: "SubjectAllocation",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SubjectAllocation",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SubjectAllocation",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SectionId",
                table: "SubjectAllocation");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectAllocation_SubjectId",
                table: "SubjectAllocation");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "SubjectAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "SubjectAllocation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "SubjectAllocation",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedOn",
                table: "SubjectAllocation",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "SubjectAllocation",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SubjectAllocation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "preExist",
                table: "SubjectAllocation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_Setup_Section_SectionId",
                table: "SubjectAllocation",
                column: "SectionId",
                principalTable: "Setup_Section",
                principalColumn: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectAllocation_Setup_Subject_SubjectId",
                table: "SubjectAllocation",
                column: "SubjectId",
                principalTable: "Setup_Subject",
                principalColumn: "SubjectId");
        }
    }
}

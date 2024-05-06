using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class ChangeInBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDateTime",
                table: "AddBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TimeWiseId",
                table: "AddBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDateTime",
                table: "AddBooking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AddBooking_TimeWiseId",
                table: "AddBooking",
                column: "TimeWiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddBooking_TimeWiseData_TimeWiseId",
                table: "AddBooking",
                column: "TimeWiseId",
                principalTable: "TimeWiseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddBooking_TimeWiseData_TimeWiseId",
                table: "AddBooking");

            migrationBuilder.DropIndex(
                name: "IX_AddBooking_TimeWiseId",
                table: "AddBooking");

            migrationBuilder.DropColumn(
                name: "FromDateTime",
                table: "AddBooking");

            migrationBuilder.DropColumn(
                name: "TimeWiseId",
                table: "AddBooking");

            migrationBuilder.DropColumn(
                name: "ToDateTime",
                table: "AddBooking");
        }
    }
}

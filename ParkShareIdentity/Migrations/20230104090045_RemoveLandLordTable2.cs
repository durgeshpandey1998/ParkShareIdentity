using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class RemoveLandLordTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddBooking_AddSpaces_AddSpaceId",
                table: "AddBooking");

            migrationBuilder.DropIndex(
                name: "IX_AddBooking_AddSpaceId",
                table: "AddBooking");

            migrationBuilder.DropColumn(
                name: "AddSpaceId",
                table: "AddBooking");

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "TimeWiseData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeOrDays",
                table: "AddSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "TimeWiseData");

            migrationBuilder.DropColumn(
                name: "TimeOrDays",
                table: "AddSpaces");

            migrationBuilder.AddColumn<int>(
                name: "AddSpaceId",
                table: "AddBooking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AddBooking_AddSpaceId",
                table: "AddBooking",
                column: "AddSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddBooking_AddSpaces_AddSpaceId",
                table: "AddBooking",
                column: "AddSpaceId",
                principalTable: "AddSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

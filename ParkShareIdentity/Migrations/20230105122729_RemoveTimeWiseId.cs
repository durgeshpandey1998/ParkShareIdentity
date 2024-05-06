using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class RemoveTimeWiseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddBooking_TimeWiseData_TimeWiseId",
                table: "AddBooking");

            migrationBuilder.RenameColumn(
                name: "TimeWiseId",
                table: "AddBooking",
                newName: "AddSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_AddBooking_TimeWiseId",
                table: "AddBooking",
                newName: "IX_AddBooking_AddSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddBooking_AddSpaces_AddSpaceId",
                table: "AddBooking",
                column: "AddSpaceId",
                principalTable: "AddSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddBooking_AddSpaces_AddSpaceId",
                table: "AddBooking");

            migrationBuilder.RenameColumn(
                name: "AddSpaceId",
                table: "AddBooking",
                newName: "TimeWiseId");

            migrationBuilder.RenameIndex(
                name: "IX_AddBooking_AddSpaceId",
                table: "AddBooking",
                newName: "IX_AddBooking_TimeWiseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddBooking_TimeWiseData_TimeWiseId",
                table: "AddBooking",
                column: "TimeWiseId",
                principalTable: "TimeWiseData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

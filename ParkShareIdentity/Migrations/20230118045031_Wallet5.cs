using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class Wallet5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddBookingId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AddBookingId",
                table: "Transactions",
                column: "AddBookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AddBooking_AddBookingId",
                table: "Transactions",
                column: "AddBookingId",
                principalTable: "AddBooking",
                principalColumn: "AddBookingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AddBooking_AddBookingId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AddBookingId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AddBookingId",
                table: "Transactions");
        }
    }
}

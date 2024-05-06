using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class Wallet6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AddBooking_AddBookingId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_OwnerWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_RenterWalletId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Recharges");

            migrationBuilder.DropTable(
                name: "TransactionDebitCredits");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OwnerWalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "RenterWalletId",
                table: "Transactions",
                newName: "WalletId");

            migrationBuilder.RenameColumn(
                name: "OwnerWalletId",
                table: "Transactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "AddBookingId",
                table: "Transactions",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_RenterWalletId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AddBookingId",
                table: "Transactions",
                newName: "IX_Transactions_BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AddBooking_BookingId",
                table: "Transactions",
                column: "BookingId",
                principalTable: "AddBooking",
                principalColumn: "AddBookingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AddBooking_BookingId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Transactions",
                newName: "RenterWalletId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transactions",
                newName: "OwnerWalletId");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Transactions",
                newName: "AddBookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_RenterWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BookingId",
                table: "Transactions",
                newName: "IX_Transactions_AddBookingId");

            migrationBuilder.CreateTable(
                name: "Recharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recharges_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDebitCredits",
                columns: table => new
                {
                    DebitCreditId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    WaletId = table.Column<int>(type: "int", nullable: false),
                    _Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDebitCredits", x => x.DebitCreditId);
                    table.ForeignKey(
                        name: "FK_TransactionDebitCredits_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OwnerWalletId",
                table: "Transactions",
                column: "OwnerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Recharges_WalletId",
                table: "Recharges",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDebitCredits_TransactionId",
                table: "TransactionDebitCredits",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AddBooking_AddBookingId",
                table: "Transactions",
                column: "AddBookingId",
                principalTable: "AddBooking",
                principalColumn: "AddBookingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_OwnerWalletId",
                table: "Transactions",
                column: "OwnerWalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_RenterWalletId",
                table: "Transactions",
                column: "RenterWalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

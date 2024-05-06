using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class Wallet2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_OwnerWalletWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_RenterWalletWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OwnerWalletWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RenterWalletWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "OwnerWalletWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RenterWalletWalletId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "RenterWalletId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerWalletId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OwnerWalletId",
                table: "Transactions",
                column: "OwnerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RenterWalletId",
                table: "Transactions",
                column: "RenterWalletId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_OwnerWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_RenterWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_OwnerWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RenterWalletId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "RenterWalletId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerWalletId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OwnerWalletWalletId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RenterWalletWalletId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OwnerWalletWalletId",
                table: "Transactions",
                column: "OwnerWalletWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RenterWalletWalletId",
                table: "Transactions",
                column: "RenterWalletWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_OwnerWalletWalletId",
                table: "Transactions",
                column: "OwnerWalletWalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_RenterWalletWalletId",
                table: "Transactions",
                column: "RenterWalletWalletId",
                principalTable: "Wallets",
                principalColumn: "WalletId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

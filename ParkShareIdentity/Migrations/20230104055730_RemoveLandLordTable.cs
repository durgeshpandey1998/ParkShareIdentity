using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class RemoveLandLordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddSpaces_Landlord_LandlordId",
                table: "AddSpaces");

            migrationBuilder.DropIndex(
                name: "IX_AddSpaces_LandlordId",
                table: "AddSpaces");

            migrationBuilder.DropColumn(
                name: "LandlordId",
                table: "AddSpaces");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MasterAddresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MasterAddresses_UserId",
                table: "MasterAddresses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterAddresses_AspNetUsers_UserId",
                table: "MasterAddresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterAddresses_AspNetUsers_UserId",
                table: "MasterAddresses");

            migrationBuilder.DropIndex(
                name: "IX_MasterAddresses_UserId",
                table: "MasterAddresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MasterAddresses");

            migrationBuilder.AddColumn<int>(
                name: "LandlordId",
                table: "AddSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AddSpaces_LandlordId",
                table: "AddSpaces",
                column: "LandlordId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddSpaces_Landlord_LandlordId",
                table: "AddSpaces",
                column: "LandlordId",
                principalTable: "Landlord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

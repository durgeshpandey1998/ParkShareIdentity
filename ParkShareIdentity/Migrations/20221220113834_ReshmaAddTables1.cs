using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class ReshmaAddTables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddSpaces_AspNetUsers_ApplicationUserId",
                table: "AddSpaces");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "AddSpaces",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AddSpaces_ApplicationUserId",
                table: "AddSpaces",
                newName: "IX_AddSpaces_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddSpaces_AspNetUsers_UserId",
                table: "AddSpaces",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddSpaces_AspNetUsers_UserId",
                table: "AddSpaces");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AddSpaces",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AddSpaces_UserId",
                table: "AddSpaces",
                newName: "IX_AddSpaces_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddSpaces_AspNetUsers_ApplicationUserId",
                table: "AddSpaces",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

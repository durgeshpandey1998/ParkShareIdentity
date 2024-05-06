using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class ParkShareM2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_applicationuserId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_applicationuserId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "applicationuserId",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "AspNetUsers",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AspNetUsers",
                table: "Vehicles",
                column: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_AspNetUsers",
                table: "Vehicles",
                column: "AspNetUsers",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_AspNetUsers",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_AspNetUsers",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AspNetUsers",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "applicationuserId",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_applicationuserId",
                table: "Vehicles",
                column: "applicationuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_applicationuserId",
                table: "Vehicles",
                column: "applicationuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

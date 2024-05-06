using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class ChangeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddSpaces_AspNetUsers_UserId",
                table: "AddSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AddSpaces_AddSpaceId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_AddSpaces_UserId",
                table: "AddSpaces");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AddSpaces");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AddSpaces");

            migrationBuilder.RenameColumn(
                name: "AddSpaceId",
                table: "Images",
                newName: "MasterAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_AddSpaceId",
                table: "Images",
                newName: "IX_Images_MasterAddressId");

            migrationBuilder.RenameColumn(
                name: "NoOfPictures",
                table: "AddSpaces",
                newName: "LandlordId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MasterAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoOfPictures",
                table: "MasterAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsVacant",
                table: "AddSpaces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Landlord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterAddressId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landlord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Landlord_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Landlord_MasterAddresses_MasterAddressId",
                        column: x => x.MasterAddressId,
                        principalTable: "MasterAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddSpaces_LandlordId",
                table: "AddSpaces",
                column: "LandlordId");

            migrationBuilder.CreateIndex(
                name: "IX_Landlord_MasterAddressId",
                table: "Landlord",
                column: "MasterAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Landlord_UserId",
                table: "Landlord",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddSpaces_Landlord_LandlordId",
                table: "AddSpaces",
                column: "LandlordId",
                principalTable: "Landlord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_MasterAddresses_MasterAddressId",
                table: "Images",
                column: "MasterAddressId",
                principalTable: "MasterAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddSpaces_Landlord_LandlordId",
                table: "AddSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_MasterAddresses_MasterAddressId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "Landlord");

            migrationBuilder.DropIndex(
                name: "IX_AddSpaces_LandlordId",
                table: "AddSpaces");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MasterAddresses");

            migrationBuilder.DropColumn(
                name: "NoOfPictures",
                table: "MasterAddresses");

            migrationBuilder.DropColumn(
                name: "IsVacant",
                table: "AddSpaces");

            migrationBuilder.RenameColumn(
                name: "MasterAddressId",
                table: "Images",
                newName: "AddSpaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_MasterAddressId",
                table: "Images",
                newName: "IX_Images_AddSpaceId");

            migrationBuilder.RenameColumn(
                name: "LandlordId",
                table: "AddSpaces",
                newName: "NoOfPictures");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AddSpaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AddSpaces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AddSpaces_UserId",
                table: "AddSpaces",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddSpaces_AspNetUsers_UserId",
                table: "AddSpaces",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AddSpaces_AddSpaceId",
                table: "Images",
                column: "AddSpaceId",
                principalTable: "AddSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

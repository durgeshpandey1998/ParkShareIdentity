using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkShareIdentity.Migrations
{
    public partial class RemoveLandLordTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "TimeWiseData");

            migrationBuilder.DropColumn(
                name: "TimeOrDays",
                table: "AddSpaces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}

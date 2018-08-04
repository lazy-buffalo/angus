using Microsoft.EntityFrameworkCore.Migrations;

namespace LazyBuffalo.Angus.Api.Migrations
{
    public partial class AddMinutesDecimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LatitudeMinutesDecimals",
                table: "GpsEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongitudeMinutesDecimals",
                table: "GpsEntries",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatitudeMinutesDecimals",
                table: "GpsEntries");

            migrationBuilder.DropColumn(
                name: "LongitudeMinutesDecimals",
                table: "GpsEntries");
        }
    }
}

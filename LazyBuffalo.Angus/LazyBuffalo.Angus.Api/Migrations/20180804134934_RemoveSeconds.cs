using Microsoft.EntityFrameworkCore.Migrations;

namespace LazyBuffalo.Angus.Api.Migrations
{
    public partial class RemoveSeconds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatitudeSecondes",
                table: "GpsEntries");

            migrationBuilder.DropColumn(
                name: "LongitudeSecondes",
                table: "GpsEntries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LatitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

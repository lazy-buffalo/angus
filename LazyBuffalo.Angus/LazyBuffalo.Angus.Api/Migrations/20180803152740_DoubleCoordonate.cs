using Microsoft.EntityFrameworkCore.Migrations;

namespace LazyBuffalo.Angus.Api.Migrations
{
    public partial class DoubleCoordonate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "LongitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeMinutes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeDeg",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeMinutes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeDeg",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LongitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LongitudeMinutes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LongitudeDeg",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LatitudeSecondes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LatitudeMinutes",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "LatitudeDeg",
                table: "GpsEntries",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}

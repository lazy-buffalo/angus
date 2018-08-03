using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LazyBuffalo.Angus.Api.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cows",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    HardwareSerial = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GpsEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    LatitudeDeg = table.Column<int>(nullable: false),
                    LatitudeMinutes = table.Column<int>(nullable: false),
                    LatitudeSecondes = table.Column<int>(nullable: false),
                    LatitudeDirection = table.Column<string>(nullable: false),
                    LongitudeDeg = table.Column<int>(nullable: false),
                    LongitudeMinutes = table.Column<int>(nullable: false),
                    LongitudeSecondes = table.Column<int>(nullable: false),
                    LongitudeDirection = table.Column<string>(nullable: false),
                    CowId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpsEntries_Cows_CowId",
                        column: x => x.CowId,
                        principalTable: "Cows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Temperature = table.Column<float>(nullable: false),
                    CowId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemperatureEntries_Cows_CowId",
                        column: x => x.CowId,
                        principalTable: "Cows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GpsEntries_CowId",
                table: "GpsEntries",
                column: "CowId");

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureEntries_CowId",
                table: "TemperatureEntries",
                column: "CowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GpsEntries");

            migrationBuilder.DropTable(
                name: "TemperatureEntries");

            migrationBuilder.DropTable(
                name: "Cows");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LazyBuffalo.Angus.Api.Migrations
{
    public partial class AddGrazings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grazings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grazings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Longitude = table.Column<double>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    GrazingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinate_Grazings_GrazingId",
                        column: x => x.GrazingId,
                        principalTable: "Grazings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinate_GrazingId",
                table: "Coordinate",
                column: "GrazingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropTable(
                name: "Grazings");
        }
    }
}

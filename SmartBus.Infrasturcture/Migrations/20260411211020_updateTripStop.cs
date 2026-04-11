using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class updateTripStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "TripStops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TripStops_LocationId",
                table: "TripStops",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TripStops_Locations_LocationId",
                table: "TripStops",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripStops_Locations_LocationId",
                table: "TripStops");

            migrationBuilder.DropIndex(
                name: "IX_TripStops_LocationId",
                table: "TripStops");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "TripStops");
        }
    }
}

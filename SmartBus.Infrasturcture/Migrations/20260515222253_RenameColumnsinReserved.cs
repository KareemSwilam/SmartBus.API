using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnsinReserved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_Locations_EndLocationId",
                table: "ReservedSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_Locations_StartLocationId",
                table: "ReservedSeats");

            migrationBuilder.DropIndex(
                name: "IX_ReservedSeats_EndLocationId",
                table: "ReservedSeats");

            migrationBuilder.DropIndex(
                name: "IX_ReservedSeats_StartLocationId",
                table: "ReservedSeats");

            migrationBuilder.RenameColumn(
                name: "StartLocationId",
                table: "ReservedSeats",
                newName: "StartStopOrder");

            migrationBuilder.RenameColumn(
                name: "EndLocationId",
                table: "ReservedSeats",
                newName: "EndStopOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartStopOrder",
                table: "ReservedSeats",
                newName: "StartLocationId");

            migrationBuilder.RenameColumn(
                name: "EndStopOrder",
                table: "ReservedSeats",
                newName: "EndLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSeats_EndLocationId",
                table: "ReservedSeats",
                column: "EndLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSeats_StartLocationId",
                table: "ReservedSeats",
                column: "StartLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_Locations_EndLocationId",
                table: "ReservedSeats",
                column: "EndLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_Locations_StartLocationId",
                table: "ReservedSeats",
                column: "StartLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

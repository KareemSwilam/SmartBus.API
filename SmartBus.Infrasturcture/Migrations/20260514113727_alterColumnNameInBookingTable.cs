using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class alterColumnNameInBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToStopOrder",
                table: "Bookings",
                newName: "StartLocationId");

            migrationBuilder.RenameColumn(
                name: "FromStopOrdere",
                table: "Bookings",
                newName: "EndLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartLocationId",
                table: "Bookings",
                newName: "ToStopOrder");

            migrationBuilder.RenameColumn(
                name: "EndLocationId",
                table: "Bookings",
                newName: "FromStopOrdere");
        }
    }
}

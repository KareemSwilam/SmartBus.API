using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnPriceSegmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopSegments_TripStops_FromStopId",
                table: "StopSegments");

            migrationBuilder.DropForeignKey(
                name: "FK_StopSegments_TripStops_ToStopId",
                table: "StopSegments");

            migrationBuilder.DropIndex(
                name: "IX_StopSegments_FromStopId",
                table: "StopSegments");

            migrationBuilder.DropIndex(
                name: "IX_StopSegments_ToStopId",
                table: "StopSegments");

            migrationBuilder.RenameColumn(
                name: "ToStopId",
                table: "StopSegments",
                newName: "ToStopOrder");

            migrationBuilder.RenameColumn(
                name: "FromStopId",
                table: "StopSegments",
                newName: "FromStopOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToStopOrder",
                table: "StopSegments",
                newName: "ToStopId");

            migrationBuilder.RenameColumn(
                name: "FromStopOrder",
                table: "StopSegments",
                newName: "FromStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSegments_FromStopId",
                table: "StopSegments",
                column: "FromStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSegments_ToStopId",
                table: "StopSegments",
                column: "ToStopId");

            migrationBuilder.AddForeignKey(
                name: "FK_StopSegments_TripStops_FromStopId",
                table: "StopSegments",
                column: "FromStopId",
                principalTable: "TripStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StopSegments_TripStops_ToStopId",
                table: "StopSegments",
                column: "ToStopId",
                principalTable: "TripStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

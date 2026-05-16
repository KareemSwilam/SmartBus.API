using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class addingSegmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StopSegments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromStopId = table.Column<int>(type: "int", nullable: false),
                    ToStopId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopSegments_TripStops_FromStopId",
                        column: x => x.FromStopId,
                        principalTable: "TripStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StopSegments_TripStops_ToStopId",
                        column: x => x.ToStopId,
                        principalTable: "TripStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_StopSegments_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StopSegments_FromStopId",
                table: "StopSegments",
                column: "FromStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSegments_ToStopId",
                table: "StopSegments",
                column: "ToStopId");

            migrationBuilder.CreateIndex(
                name: "IX_StopSegments_TripId",
                table: "StopSegments",
                column: "TripId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopSegments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class addingAVGRAttingColumnToCompanyTripDriverTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AVGRating",
                table: "Trips",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AVGRating",
                table: "Drivers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AVGRating",
                table: "Companies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AVGRating",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "AVGRating",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "AVGRating",
                table: "Companies");
        }
    }
}

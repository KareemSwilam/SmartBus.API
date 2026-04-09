using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class addingColumnToLoctionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpenStreetMapId",
                table: "Locations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenStreetMapId",
                table: "Locations");
        }
    }
}

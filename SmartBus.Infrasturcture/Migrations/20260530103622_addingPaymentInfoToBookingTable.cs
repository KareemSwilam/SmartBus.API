using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBus.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class addingPaymentInfoToBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "Bookings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentReferenceNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PaymentReferenceNumber",
                table: "Bookings");
        }
    }
}

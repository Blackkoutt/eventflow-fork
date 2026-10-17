using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTypeAsEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "Reservation",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "HallRent",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "PaymentTypeId",
                table: "EventPass",
                newName: "PaymentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Reservation",
                newName: "PaymentTypeId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "HallRent",
                newName: "PaymentTypeId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "EventPass",
                newName: "PaymentTypeId");
        }
    }
}

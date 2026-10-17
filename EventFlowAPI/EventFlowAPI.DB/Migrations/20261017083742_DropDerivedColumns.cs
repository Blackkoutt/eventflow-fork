using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class DropDerivedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFestivalReservation",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "TotalArea",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Festival");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Event");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFestivalReservation",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "HallRent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalArea",
                table: "Hall",
                type: "NUMERIC(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Festival",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Event",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1,
                column: "Duration",
                value: -3600L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2,
                column: "Duration",
                value: -10800L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3,
                column: "Duration",
                value: -7200L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4,
                column: "Duration",
                value: -10800L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5,
                column: "Duration",
                value: -3600L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6,
                column: "Duration",
                value: -7200L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7,
                column: "Duration",
                value: -10800L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8,
                column: "Duration",
                value: 9000L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9,
                column: "Duration",
                value: 7200L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10,
                column: "Duration",
                value: 7200L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11,
                column: "Duration",
                value: 7200L);

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12,
                column: "Duration",
                value: 7200L);

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1,
                column: "Duration",
                value: -2682000L);

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2,
                column: "Duration",
                value: -2685600L);

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3,
                column: "Duration",
                value: -2602800L);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 1,
                column: "TotalArea",
                value: 308m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 2,
                column: "TotalArea",
                value: 198m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 3,
                column: "TotalArea",
                value: 99m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 4,
                column: "TotalArea",
                value: 153m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 5,
                column: "TotalArea",
                value: 108m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 6,
                column: "TotalArea",
                value: 198m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 7,
                column: "TotalArea",
                value: 99m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 8,
                column: "TotalArea",
                value: 153m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 9,
                column: "TotalArea",
                value: 308m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 10,
                column: "TotalArea",
                value: 198m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 11,
                column: "TotalArea",
                value: 153m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 12,
                column: "TotalArea",
                value: 308m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 13,
                column: "TotalArea",
                value: 198m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 14,
                column: "TotalArea",
                value: 99m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 15,
                column: "TotalArea",
                value: 153m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 16,
                column: "TotalArea",
                value: 99m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 17,
                column: "TotalArea",
                value: 198m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 18,
                column: "TotalArea",
                value: 308m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 19,
                column: "TotalArea",
                value: 99m);

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 20,
                column: "TotalArea",
                value: 153m);

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 1,
                column: "Duration",
                value: 28800L);

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 2,
                column: "Duration",
                value: 14400L);

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 3,
                column: "Duration",
                value: 7200L);

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 4,
                column: "Duration",
                value: 3600L);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsFestivalReservation",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsFestivalReservation",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsFestivalReservation",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsFestivalReservation",
                value: false);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsFestivalReservation",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsFestivalReservation",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsFestivalReservation",
                value: true);

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsFestivalReservation",
                value: true);
        }
    }
}

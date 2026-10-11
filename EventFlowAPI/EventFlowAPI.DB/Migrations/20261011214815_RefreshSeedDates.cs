using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class RefreshSeedDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 13, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 15, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 15, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 26, 2, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 10, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 10, 1, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 9, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 9, 2, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 8, 5, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 8, 3, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 7, 4, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2027, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2027, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2027, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 15, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 12, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 13, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 15, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "PublicationDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 13, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 15, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 12, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 11, 13, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2026, 12, 14, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                column: "RegisteredDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "RegisteredDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "RegisteredDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4",
                column: "RegisteredDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5",
                column: "RegisteredDate",
                value: new DateTime(2026, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 2, 2, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 3, 17, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 17, 1, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 3, 16, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 16, 2, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 3, 15, 5, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 15, 3, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Event",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 3, 14, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 14, 4, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Festival",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "PaymentDate", "RentDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 22, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "PublicationDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "EndDate", "PaymentDate", "ReservationDate", "StartDate" },
                values: new object[] { new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                column: "RegisteredDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                column: "RegisteredDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "3",
                column: "RegisteredDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4",
                column: "RegisteredDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5",
                column: "RegisteredDate",
                value: new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

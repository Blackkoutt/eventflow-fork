using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGeneratedFileNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketImagesFolder",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "TicketPDFFileName",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "HallRentPDFName",
                table: "HallRent");

            migrationBuilder.DropColumn(
                name: "HallViewFileName",
                table: "Hall");

            migrationBuilder.DropColumn(
                name: "EventPassJPGName",
                table: "EventPass");

            migrationBuilder.DropColumn(
                name: "EventPassPDFName",
                table: "EventPass");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TicketImagesFolder",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketPDFFileName",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HallRentPDFName",
                table: "HallRent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HallViewFileName",
                table: "Hall",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventPassJPGName",
                table: "EventPass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventPassPDFName",
                table: "EventPass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EventPassJPGName", "EventPassPDFName" },
                values: new object[] { "eventflow_karnet_b00ca94a-e6b2-4d2e-b270-244b3e76048d.jpg", "eventflow_karnet_b00ca94a-e6b2-4d2e-b270-244b3e76048d.pdf" });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EventPassJPGName", "EventPassPDFName" },
                values: new object[] { "eventflow_karnet_766245b4-8c08-49dd-9480-2606aaa590be.jpg", "eventflow_karnet_766245b4-8c08-49dd-9480-2606aaa590be.pdf" });

            migrationBuilder.UpdateData(
                table: "EventPass",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EventPassJPGName", "EventPassPDFName" },
                values: new object[] { "eventflow_karnet_33610a0d-a1b7-4700-bffe-9e334b977e6a.jpg", "eventflow_karnet_33610a0d-a1b7-4700-bffe-9e334b977e6a.pdf" });

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 1,
                column: "HallViewFileName",
                value: "sala_5_5_1.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 2,
                column: "HallViewFileName",
                value: "sala_5_5_2.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 3,
                column: "HallViewFileName",
                value: "sala_5_5_3.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 4,
                column: "HallViewFileName",
                value: "sala_5_5_4.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 5,
                column: "HallViewFileName",
                value: "sala_5_5_5.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 6,
                column: "HallViewFileName",
                value: "sala_5_5_6.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 7,
                column: "HallViewFileName",
                value: "sala_5_5_7.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 8,
                column: "HallViewFileName",
                value: "sala_5_5_8.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 9,
                column: "HallViewFileName",
                value: "sala_5_5_9.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 10,
                column: "HallViewFileName",
                value: "sala_5_5_10.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 11,
                column: "HallViewFileName",
                value: "sala_5_5_11.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 12,
                column: "HallViewFileName",
                value: "sala_5_5_12.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 13,
                column: "HallViewFileName",
                value: "sala_5_5_13.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 14,
                column: "HallViewFileName",
                value: "sala_5_5_14.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 15,
                column: "HallViewFileName",
                value: "sala_5_5_15.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 16,
                column: "HallViewFileName",
                value: "sala_5_5_16.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 17,
                column: "HallViewFileName",
                value: "sala_5_5_17.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 18,
                column: "HallViewFileName",
                value: "sala_5_5_18.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 19,
                column: "HallViewFileName",
                value: "sala_5_5_19.pdf");

            migrationBuilder.UpdateData(
                table: "Hall",
                keyColumn: "Id",
                keyValue: 20,
                column: "HallViewFileName",
                value: "sala_5_5_20.pdf");

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 1,
                column: "HallRentPDFName",
                value: "wynajem_sali_1.pdf");

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 2,
                column: "HallRentPDFName",
                value: "wynajem_sali_2.pdf");

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 3,
                column: "HallRentPDFName",
                value: "wynajem_sali_3.pdf");

            migrationBuilder.UpdateData(
                table: "HallRent",
                keyColumn: "Id",
                keyValue: 4,
                column: "HallRentPDFName",
                value: "wynajem_sali_4.pdf");

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "53538b58-f885-4f4a-b675-a4aa4063ccf3", "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "ed8b9230-223b-4609-8d13-aa6017edad09", "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "f9a076c4-3475-4a28-a60c-6e0e3c03731a", "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "de1d6773-f027-4888-996a-0296e5c52708", "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "0b74ec7b-933b-4163-afa5-e0997681dccd", "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "0b74ec7b-933b-4163-afa5-e0997681dccd", "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "806cade1-2685-43dc-8cfc-682fc4229db6", "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf" });

            migrationBuilder.UpdateData(
                table: "Reservation",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "TicketImagesFolder", "TicketPDFFileName" },
                values: new object[] { "806cade1-2685-43dc-8cfc-682fc4229db6", "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf" });
        }
    }
}

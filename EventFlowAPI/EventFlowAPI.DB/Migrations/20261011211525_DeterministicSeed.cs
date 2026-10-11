using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventFlowAPI.DB.Migrations
{
    /// <inheritdoc />
    public partial class DeterministicSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGuGsnP/NPzhnWuATv9aP1dULQrjmPSUU5Lnaftfuse3AiOfXJXbIF7N3Cad2JluJQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENRzXjomXhbu4D9ZcYubXQOOyo5yl/gp6gjzTvWYGyFWB4zpo6txFqMrRYmWzxh/ow==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE4HQIVivtuLYM4Fwd7P462tG2SscYZwnvDj07rYCz755XCB3O7mDcjtHBT+fdeSrQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEsid3fU3R/GHG+IT0hkHQLU1yIbx7kE6ZQ2IMX7/ZGpkmvhNZ4UojrRpKJALHM1Aw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO8tEn3je5QrqtBhReVLDGGi49kxRWDYNbrMJVhZ7nwzH3X1XYcNckGnHN7p5NnTqg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK4/hlBoE1k2Sdq8n2DYNHysLOR3Fg/7anvL/W7qoX0GxzJBNLQqG79m14mPJ6iwrw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG6rZhspfbU8XJjYA5dsJzuNC6oO71cVoB6GJ3pm3voqJFOM7pvY0OsDo5PZDIuzxA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAlgQ4Qs/GFmn3hHt58p/CNdbRcnd2+tGvTWqNDxznjanmwir/VeibWWI3B9jZhf8g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEErLVney/2f4Wm11sn7nK8cNngA8P3qvHSertKvrBHNSE/000BYDaS+/F5tYAhMIFQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFjlpJmB1mkCX/au5X5r7DmJLIjfboZ0uKo0vyl9ZfqXDpEWpvdISbzfLhblOMN/uQ==");
        }
    }
}

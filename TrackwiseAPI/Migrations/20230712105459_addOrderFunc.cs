using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class addOrderFunc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4236));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4238));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4139));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4150));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4152));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4264));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4265));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "4",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4324));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "5",
                column: "Date",
                value: new DateTime(2023, 7, 12, 12, 54, 58, 911, DateTimeKind.Local).AddTicks(4326));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8715));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8716));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8717));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8629));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8639));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "1",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8736));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "2",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8738));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "3",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8740));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "4",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8741));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: "5",
                column: "Date",
                value: new DateTime(2023, 7, 3, 18, 19, 10, 446, DateTimeKind.Local).AddTicks(8742));
        }
    }
}

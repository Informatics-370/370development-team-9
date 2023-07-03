using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3223));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3228));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3229));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3123));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3134));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3136));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3254));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3256));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3258));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3259));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 28, 17, 46, 59, 131, DateTimeKind.Local).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Product_ID",
                keyValue: 1,
                column: "Product_Name",
                value: "Fuel Pump");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(237));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(239));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(240));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(148));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(256));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 26, 12, 24, 13, 342, DateTimeKind.Local).AddTicks(259));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Product_ID",
                keyValue: 1,
                column: "Product_Name",
                value: "Feul Pump");
        }
    }
}

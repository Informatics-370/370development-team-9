using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Customer_ID",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Customer_ID);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Customer_ID", "Email", "LastName", "Name", "Password" },
                values: new object[,]
                {
                    { "1", "johndoe@gmail.com", "Doe", "John", "john123" },
                    { "2", "janesmith@gmail.com", "Smith", "Jane", "jane123" },
                    { "3", "joemama@gmail.com", "Mama", "Joe", "joe123" }
                });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5317));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5319));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5320));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5341));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5343));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5344));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5345));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5347));

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Order_ID", "Customer_ID", "Date", "Status", "Total" },
                values: new object[,]
                {
                    { 1, "1", new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5231), "Ordered", 2897.0 },
                    { 2, "2", new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5242), "Ordered", 2997.0 },
                    { 3, "3", new DateTime(2023, 6, 29, 14, 19, 5, 582, DateTimeKind.Local).AddTicks(5245), "Ordered", 2998.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_ID",
                table: "Orders",
                column: "Customer_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_Customer_ID",
                table: "Orders",
                column: "Customer_ID",
                principalTable: "Customers",
                principalColumn: "Customer_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_Customer_ID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Customer_ID",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Customer_ID",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9160));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9197));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9198));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9200));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9201));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 29, 14, 16, 35, 739, DateTimeKind.Local).AddTicks(9202));
        }
    }
}

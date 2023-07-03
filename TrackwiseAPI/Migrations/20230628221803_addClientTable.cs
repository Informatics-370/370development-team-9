using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class addClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Client_ID",
                table: "jobs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Client_ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Client_ID);
                });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6001));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6003));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(5871));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(5920));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6029));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6031));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 18, 3, 362, DateTimeKind.Local).AddTicks(6032));

            migrationBuilder.CreateIndex(
                name: "IX_jobs_Client_ID",
                table: "jobs",
                column: "Client_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_Clients_Client_ID",
                table: "jobs",
                column: "Client_ID",
                principalTable: "Clients",
                principalColumn: "Client_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_Clients_Client_ID",
                table: "jobs");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_jobs_Client_ID",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Client_ID",
                table: "jobs");

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(554));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(555));

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Invoice_number",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(557));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(453));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(472));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Order_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(578));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(579));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(581));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(582));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Payment_ID",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2023, 6, 29, 0, 15, 37, 100, DateTimeKind.Local).AddTicks(583));
        }
    }
}

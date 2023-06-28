using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class dropClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Client_ID",
                table: "jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Client_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
    }
}

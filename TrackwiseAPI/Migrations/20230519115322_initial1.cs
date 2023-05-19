using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrackwiseAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Admin_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Admin_ID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Client_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Client_ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "DriverStatuses",
                columns: table => new
                {
                    Driver_Status_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverStatuses", x => x.Driver_Status_ID);
                });

            migrationBuilder.CreateTable(
                name: "HelpCategories",
                columns: table => new
                {
                    Help_Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpCategories", x => x.Help_Category_ID);
                });

            migrationBuilder.CreateTable(
                name: "jobsStatus",
                columns: table => new
                {
                    Job_Status_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobsStatus", x => x.Job_Status_ID);
                });

            migrationBuilder.CreateTable(
                name: "jobTypes",
                columns: table => new
                {
                    Job_Type_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobTypes", x => x.Job_Type_ID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Payment_Type_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descrtipion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Payment_Type_ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Product_Type_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Product_Type_ID);
                });

            migrationBuilder.CreateTable(
                name: "TrailerStatuses",
                columns: table => new
                {
                    Trailer_Status_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerStatuses", x => x.Trailer_Status_ID);
                });

            migrationBuilder.CreateTable(
                name: "TrailerTypes",
                columns: table => new
                {
                    Trailer_Type_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerTypes", x => x.Trailer_Type_ID);
                });

            migrationBuilder.CreateTable(
                name: "TruckStatuses",
                columns: table => new
                {
                    Truck_Status_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckStatuses", x => x.Truck_Status_ID);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Supplier_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Supplier_ID);
                    table.ForeignKey(
                        name: "FK_Suppliers_Admins_Admin_ID",
                        column: x => x.Admin_ID,
                        principalTable: "Admins",
                        principalColumn: "Admin_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_Customer_ID",
                        column: x => x.Customer_ID,
                        principalTable: "Customers",
                        principalColumn: "Customer_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Driver_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Driver_Status_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Driver_ID);
                    table.ForeignKey(
                        name: "FK_Drivers_DriverStatuses_Driver_Status_ID",
                        column: x => x.Driver_Status_ID,
                        principalTable: "DriverStatuses",
                        principalColumn: "Driver_Status_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Helps",
                columns: table => new
                {
                    Help_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Help_Category_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helps", x => x.Help_ID);
                    table.ForeignKey(
                        name: "FK_Helps_HelpCategories_Help_Category_ID",
                        column: x => x.Help_Category_ID,
                        principalTable: "HelpCategories",
                        principalColumn: "Help_Category_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    Job_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pickup_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dropoff_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Client_ID = table.Column<int>(type: "int", nullable: false),
                    Admin_ID = table.Column<int>(type: "int", nullable: false),
                    Job_Type_ID = table.Column<int>(type: "int", nullable: false),
                    Job_Status_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.Job_ID);
                    table.ForeignKey(
                        name: "FK_jobs_Admins_Admin_ID",
                        column: x => x.Admin_ID,
                        principalTable: "Admins",
                        principalColumn: "Admin_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_jobs_Clients_Client_ID",
                        column: x => x.Client_ID,
                        principalTable: "Clients",
                        principalColumn: "Client_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_jobs_jobTypes_Job_Type_ID",
                        column: x => x.Job_Type_ID,
                        principalTable: "jobTypes",
                        principalColumn: "Job_Type_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_jobs_jobsStatus_Job_Status_ID",
                        column: x => x.Job_Status_ID,
                        principalTable: "jobsStatus",
                        principalColumn: "Job_Status_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Product_Category_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product_Type_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Product_Category_ID);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductTypes_Product_Type_ID",
                        column: x => x.Product_Type_ID,
                        principalTable: "ProductTypes",
                        principalColumn: "Product_Type_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    TrailerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Trailer_License = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Trailer_Type_ID = table.Column<int>(type: "int", nullable: false),
                    Trailer_Status_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.TrailerID);
                    table.ForeignKey(
                        name: "FK_Trailers_TrailerStatuses_Trailer_Status_ID",
                        column: x => x.Trailer_Status_ID,
                        principalTable: "TrailerStatuses",
                        principalColumn: "Trailer_Status_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trailers_TrailerTypes_Trailer_Type_ID",
                        column: x => x.Trailer_Type_ID,
                        principalTable: "TrailerTypes",
                        principalColumn: "Trailer_Type_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Truck_License = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Truck_Status_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckID);
                    table.ForeignKey(
                        name: "FK_Trucks_TruckStatuses_Truck_Status_ID",
                        column: x => x.Truck_Status_ID,
                        principalTable: "TruckStatuses",
                        principalColumn: "Truck_Status_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Invoice_number = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total_Amount = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Invoice_number);
                    table.ForeignKey(
                        name: "FK_Invoice_Orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Orders",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Payment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount_paid = table.Column<double>(type: "float", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: false),
                    Payment_Type_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Payment_ID);
                    table.ForeignKey(
                        name: "FK_Payment_Orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Orders",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_PaymentType_Payment_Type_ID",
                        column: x => x.Payment_Type_ID,
                        principalTable: "PaymentType",
                        principalColumn: "Payment_Type_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product_Price = table.Column<double>(type: "float", nullable: false),
                    Product_Category_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Product_ID);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_Product_Category_ID",
                        column: x => x.Product_Category_ID,
                        principalTable: "ProductCategories",
                        principalColumn: "Product_Category_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deliveries",
                columns: table => new
                {
                    Delivery_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    weight = table.Column<double>(type: "float", nullable: false),
                    Job_ID = table.Column<int>(type: "int", nullable: false),
                    TruckID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveries", x => x.Delivery_ID);
                    table.ForeignKey(
                        name: "FK_deliveries_Trucks_TruckID",
                        column: x => x.TruckID,
                        principalTable: "Trucks",
                        principalColumn: "TruckID");
                    table.ForeignKey(
                        name: "FK_deliveries_jobs_Job_ID",
                        column: x => x.Job_ID,
                        principalTable: "jobs",
                        principalColumn: "Job_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_ID = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    reorder_total = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Products_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Lines",
                columns: table => new
                {
                    Orderid = table.Column<int>(type: "int", nullable: false),
                    Productid = table.Column<int>(type: "int", nullable: false),
                    Order_line_ID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Lines", x => new { x.Orderid, x.Productid });
                    table.ForeignKey(
                        name: "FK_Order_Lines_Orders_Orderid",
                        column: x => x.Orderid,
                        principalTable: "Orders",
                        principalColumn: "Order_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Lines_Products_Productid",
                        column: x => x.Productid,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Suppliers",
                columns: table => new
                {
                    Supplierid = table.Column<int>(type: "int", nullable: false),
                    Productid = table.Column<int>(type: "int", nullable: false),
                    Product_Supplier_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Suppliers", x => new { x.Supplierid, x.Productid });
                    table.ForeignKey(
                        name: "FK_Product_Suppliers_Products_Productid",
                        column: x => x.Productid,
                        principalTable: "Products",
                        principalColumn: "Product_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Suppliers_Suppliers_Supplierid",
                        column: x => x.Supplierid,
                        principalTable: "Suppliers",
                        principalColumn: "Supplier_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivery_Assignments",
                columns: table => new
                {
                    Deliveryid = table.Column<int>(type: "int", nullable: false),
                    Driverid = table.Column<int>(type: "int", nullable: false),
                    Delivery_Assignment_ID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery_Assignments", x => new { x.Driverid, x.Deliveryid });
                    table.ForeignKey(
                        name: "FK_Delivery_Assignments_Drivers_Driverid",
                        column: x => x.Driverid,
                        principalTable: "Drivers",
                        principalColumn: "Driver_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Delivery_Assignments_deliveries_Deliveryid",
                        column: x => x.Deliveryid,
                        principalTable: "deliveries",
                        principalColumn: "Delivery_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Admin_ID", "Email", "Lastname", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "hanruduplessis@gmail.com", "du Plessis", "Hanru", "hanru123" },
                    { 2, "admin@gmail.com", "admin", "admin", "admin123" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Customer_ID", "Email", "LastName", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "johndoe@gmail.com", "Doe", "John", "john123" },
                    { 2, "janesmith@gmail.com", "Smith", "Jane", "jane123" },
                    { 3, "joemama@gmail.com", "Mama", "Joe", "joe123" }
                });

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "Payment_Type_ID", "Descrtipion", "Name" },
                values: new object[,]
                {
                    { 1, "Customer paid with credit card", "Credit Card" },
                    { 2, "Customer paid with EFT", "EFT" },
                    { 3, "Customer paid with cash", "Cash" }
                });

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "Product_Type_ID", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Product has trailer components", "Truck" },
                    { 2, "Product has truck components", "Trailer" }
                });

            migrationBuilder.InsertData(
                table: "TrailerStatuses",
                columns: new[] { "Trailer_Status_ID", "Description", "Status" },
                values: new object[,]
                {
                    { 1, "Trailer is available for job", "Available" },
                    { 2, "Trailer is busy with a job", "Unavailable" },
                    { 3, "Trailer is undergoing maintenace", "Under Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "TrailerTypes",
                columns: new[] { "Trailer_Type_ID", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Coal transportation trailer", "Coal" },
                    { 2, "Fuel transportation trailer", "Feul" }
                });

            migrationBuilder.InsertData(
                table: "TruckStatuses",
                columns: new[] { "Truck_Status_ID", "Description", "Status" },
                values: new object[,]
                {
                    { 1, "Truck is available for job", "Available" },
                    { 2, "Truck is busy with a job", "Unavailable" },
                    { 3, "Truck is undergoing maintenace", "Under Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Order_ID", "Customer_ID", "Date", "Status", "Total" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4864), "Ordered", 2897.0 },
                    { 2, 2, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4876), "Ordered", 2997.0 },
                    { 3, 3, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4877), "Ordered", 2998.0 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Product_Category_ID", "Description", "Name", "Product_Type_ID" },
                values: new object[,]
                {
                    { 1, "products for engines", "Engine", 1 },
                    { 2, "products for transmissions", "Transmission", 1 },
                    { 3, "products for suspensions", "Suspension", 1 },
                    { 4, "products for electrical", "Electrical", 1 },
                    { 5, "products for electrical", "Electrical", 2 },
                    { 6, "products for body", "Body", 1 },
                    { 7, "products for brakes", "Brake", 1 },
                    { 8, "products for wheels", "Wheel", 1 },
                    { 9, "bolts,nuts ect..", "Consumables", 1 },
                    { 10, "bolts,nuts ect..", "Consumables", 2 }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Supplier_ID", "Admin_ID", "Email", "Name" },
                values: new object[,]
                {
                    { 1, 1, "abc@gmail.com", "ABC Suppliers" },
                    { 2, 2, "xyz@gmail.com", "XYZ Suppliers" }
                });

            migrationBuilder.InsertData(
                table: "Invoice",
                columns: new[] { "Invoice_number", "Date", "Order_ID", "Total_Amount" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4916), 1, 200.5 },
                    { 2, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4917), 2, 75.200000000000003 },
                    { 3, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4917), 3, 450.0 }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Payment_ID", "Date", "Order_ID", "Payment_Type_ID", "amount_paid" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4927), 1, 1, 150.5 },
                    { 2, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4928), 1, 2, 50.0 },
                    { 3, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4929), 2, 3, 75.200000000000003 },
                    { 4, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4929), 3, 1, 200.0 },
                    { 5, new DateTime(2023, 5, 19, 13, 53, 22, 825, DateTimeKind.Local).AddTicks(4930), 3, 2, 250.0 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Product_ID", "Product_Category_ID", "Product_Description", "Product_Name", "Product_Price" },
                values: new object[,]
                {
                    { 1, 4, "FUEL PRIMER PUMP/K5", "Feul Pump", 999.0 },
                    { 2, 9, "SEAL RING MB-S48", "SEAL RING", 899.0 },
                    { 3, 7, "CLUTCH MASTER CYL 24mm SIDE MOUNT-S10", "CLUTCH", 1499.0 },
                    { 4, 10, "SAF AXLE NUT LEFT M75x1.5 (85mm)", "AXLE NUT", 1199.0 },
                    { 5, 10, "BEARING INN ROCKWELL TM 218248/210/HM", "BEARING", 9.9900000000000002 },
                    { 6, 9, "SEAL OIL STEERING M/B AXOR-S46", "SEAL OIL", 119.98999999999999 },
                    { 7, 7, "BRAKEPAD TO FIT MAN TGS/TGX WVA29279", "BRAKEPAD", 799.0 },
                    { 8, 1, "FAN BELT 9PK2300-U7", "FAN BELT", 455.0 }
                });

            migrationBuilder.InsertData(
                table: "Order_Lines",
                columns: new[] { "Orderid", "Productid", "Order_line_ID", "Quantity", "SubTotal" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 1998.0 },
                    { 1, 2, 2, 1, 899.0 },
                    { 2, 1, 3, 3, 2997.0 },
                    { 3, 3, 4, 2, 2998.0 }
                });

            migrationBuilder.InsertData(
                table: "Product_Suppliers",
                columns: new[] { "Productid", "Supplierid", "Product_Supplier_ID" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 2, 2, 3 },
                    { 3, 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_Job_ID",
                table: "deliveries",
                column: "Job_ID");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_TruckID",
                table: "deliveries",
                column: "TruckID");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_Assignments_Deliveryid",
                table: "Delivery_Assignments",
                column: "Deliveryid");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_Driver_Status_ID",
                table: "Drivers",
                column: "Driver_Status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Helps_Help_Category_ID",
                table: "Helps",
                column: "Help_Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_Product_ID",
                table: "Inventory",
                column: "Product_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Order_ID",
                table: "Invoice",
                column: "Order_ID");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_Admin_ID",
                table: "jobs",
                column: "Admin_ID");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_Client_ID",
                table: "jobs",
                column: "Client_ID");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_Job_Status_ID",
                table: "jobs",
                column: "Job_Status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_Job_Type_ID",
                table: "jobs",
                column: "Job_Type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Lines_Productid",
                table: "Order_Lines",
                column: "Productid");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_ID",
                table: "Orders",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Order_ID",
                table: "Payment",
                column: "Order_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Payment_Type_ID",
                table: "Payment",
                column: "Payment_Type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Suppliers_Productid",
                table: "Product_Suppliers",
                column: "Productid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Product_Type_ID",
                table: "ProductCategories",
                column: "Product_Type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Product_Category_ID",
                table: "Products",
                column: "Product_Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Admin_ID",
                table: "Suppliers",
                column: "Admin_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_Trailer_Status_ID",
                table: "Trailers",
                column: "Trailer_Status_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_Trailer_Type_ID",
                table: "Trailers",
                column: "Trailer_Type_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_Truck_Status_ID",
                table: "Trucks",
                column: "Truck_Status_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Delivery_Assignments");

            migrationBuilder.DropTable(
                name: "Helps");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Order_Lines");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Product_Suppliers");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "deliveries");

            migrationBuilder.DropTable(
                name: "HelpCategories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "TrailerStatuses");

            migrationBuilder.DropTable(
                name: "TrailerTypes");

            migrationBuilder.DropTable(
                name: "DriverStatuses");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "TruckStatuses");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "jobTypes");

            migrationBuilder.DropTable(
                name: "jobsStatus");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}

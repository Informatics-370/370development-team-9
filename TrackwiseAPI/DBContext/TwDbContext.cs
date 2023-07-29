using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.DBContext
{
    public class TwDbContext : IdentityDbContext<AppUser>
    {
        public TwDbContext(DbContextOptions<TwDbContext> options) : base(options) { }

        //
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverStatus> DriverStatuses { get; set; }
        public DbSet<Help> Helps { get; set; }
        public DbSet<HelpCategory> HelpCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<TrailerStatus> TrailerStatuses { get; set; }
        public DbSet<TrailerType> TrailerTypes { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<TruckStatus> TruckStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Line> Order_Lines { get; set; }
        public DbSet<Product_Supplier> Product_Suppliers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobStatus> JobsStatus { get; set; }
        public DbSet<JobType> JobTypes { get; set; }


        /// 
        /// 
        /// 
        /// 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            //Supplier and product many-many
            modelBuilder.Entity<Product_Supplier>()
            .HasKey(ps => new { ps.Supplierid, ps.Productid });

            modelBuilder.Entity<Product_Supplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.Product_Suppliers)
                .HasForeignKey(ps => ps.Supplierid);

            modelBuilder.Entity<Product_Supplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.Product_Suppliers)
                .HasForeignKey(ps => ps.Productid);
             
            //Product 1-1 with inventory
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.Product_ID);

            //Product 1-many with product_type
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.Product_Type_ID);

            //Product 1-many with product_category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.Product_Category_ID);

            //Product has a many-many relationship with Order_Line
            modelBuilder.Entity<Order_Line>()
                .HasKey(ol => new { ol.Orderid, ol.Productid });

            modelBuilder.Entity<Order_Line>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(ol => ol.Orderid);

            modelBuilder.Entity<Order_Line>()
                .HasOne(ol => ol.Product)
                .WithMany(p => p.OrderLines)
                .HasForeignKey(ol => ol.Productid);

            //Order has a many to one relationshup with Customer
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.orders)
                .HasForeignKey(o => o.Customer_ID);

            //Order and invoice has a 1-many relationship
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithMany(o => o.invoices)
                .HasForeignKey(i => i.Order_ID);

            //Order and Payment has a 1-many relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.payments)
                .HasForeignKey(p => p.Order_ID);

            //Payment and Payment_Type has a one-many relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentType)
                .WithMany(pt => pt.Payments)
                .HasForeignKey(p => p.Payment_Type_ID);

            //Truck and TruckStatus has a many-one relationship
            modelBuilder.Entity<Truck>()
                .HasOne(t => t.TruckStatus)
                .WithMany(ts => ts.Trucks)
                .HasForeignKey(t => t.Truck_Status_ID);

            //Trailer and TrailerStatus has a many-one relationship
            modelBuilder.Entity<Trailer>()
                .HasOne(t => t.TrailerStatus)
                .WithMany(ts => ts.Trailers)
                .HasForeignKey(t => t.Trailer_Status_ID);

            //Trailer and TrailerType has a many-one relationship
            modelBuilder.Entity<Trailer>()
                .HasOne(t => t.TrailerType)
                .WithMany(tt => tt.Trailers)
                .HasForeignKey(t => t.Trailer_Type_ID);

            ////////////////////////////////////////////////////////////////////////////////

            //Job and Delivery has a one-many
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Job)
                .WithMany(j => j.Deliveries)
                .HasForeignKey(d => d.Job_ID);


            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Driver)
                .WithMany(j => j.Deliveries)
                .HasForeignKey(d => d.Driver_ID);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Trailer)
                .WithMany(j => j.Deliveries)
                .HasForeignKey(d => d.TrailerID);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Truck)
                .WithMany(j => j.Deliveries)
                .HasForeignKey(d => d.TruckID);


            //Job and JobType has a many-one
            modelBuilder.Entity<Job>()
                .HasOne(j => j.JobType)
                .WithMany(jt => jt.Jobs)
                .HasForeignKey(j => j.Job_Type_ID);

            //Job and JobStatus has a many-one
            modelBuilder.Entity<Job>()
                .HasOne(j => j.JobStatus)
                .WithMany(js => js.Jobs)
                .HasForeignKey(j => j.Job_Status_ID);
                
            //Client and Job has a one-many
            /*
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Client)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.Client_ID);
            */

            //Admin and Job has a one-many
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Admin)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.Admin_ID);


            ////////////////////////////////////////////////////////////////////////////////

            //Driver and DriverStatus has a many-one
            modelBuilder.Entity<Driver>()
                .HasOne(j => j.DriverStatus)
                .WithMany(js => js.Drivers)
                .HasForeignKey(j => j.Driver_Status_ID);

            //Truck and TruckStatus has a many-one
            modelBuilder.Entity<Truck>()
                .HasOne(t => t.TruckStatus)
                .WithMany(ts => ts.Trucks)
                .HasForeignKey(t => t.Truck_Status_ID);

            //Trailer and TrailerStatus has a many-one
            modelBuilder.Entity<Trailer>()
                .HasOne(t => t.TrailerStatus)
                .WithMany(ts => ts.Trailers)
                .HasForeignKey(t => t.Trailer_Status_ID);

            //Trailer and TrailerType has a many-one
            modelBuilder.Entity<Trailer>()
                .HasOne(t => t.TrailerType)
                .WithMany(ts => ts.Trailers)
                .HasForeignKey(t => t.Trailer_Type_ID);

            //
            // adding some data
            // adding some data
            //

            modelBuilder.Entity<JobType>().HasData(
                new JobType { Job_Type_ID= "1", Name = "Coal", Description = "Transporting coal" }
                );

            modelBuilder.Entity<JobStatus>().HasData(
                new JobStatus { Job_Status_ID = "1", Name = "In-opperation", Description = "Transporting in progress" },
                new JobStatus { Job_Status_ID = "2", Name = "Complete", Description = "Transporting complete" }
                );

            //job data
            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    Job_ID = "1",
                    StartDate = DateTime.ParseExact("2023-07-23 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DueDate = DateTime.ParseExact("2023-07-31 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    Pickup_Location = "lephalale, Limpopo, South Africa",
                    Dropoff_Location = "Bela-Bela, Limpopo, South Africa",
                    Total_Weight = 35.00,
                    //Client_ID = "1",
                    Admin_ID = "1",
                    Job_Type_ID = "1",
                    Job_Status_ID = "2"
                },
                new Job
                {
                    Job_ID = "2",
                    //StartDate = new DateTime(2023, 8, 1),
                    //DueDate = new DateTime(2023, 8, 11),
                    Pickup_Location = "lephalale, Limpopo, South Africa",
                    Dropoff_Location = "Bela-Bela, Limpopo, South Africa",
                    Total_Weight = 105.00,
                    //Client_ID = "1",
                    Admin_ID = "1",
                    Job_Type_ID = "1",
                    Job_Status_ID = "2"                    
                }
            );

            // Add mock data for Delivery
            modelBuilder.Entity<Delivery>().HasData(
                //JOB1
                new Delivery
                {
                    Delivery_ID = "1", Delivery_Weight = 35.00, Job_ID = "1", Driver_ID = "1", TruckID = "1", TrailerID = "1"
                },
                //JOB2 1driver 3 trips
                new Delivery
                {
                    Delivery_ID = "2", Delivery_Weight = 35.00, Job_ID = "2", Driver_ID = "1", TruckID = "1", TrailerID = "1"
                },
                new Delivery
                {
                    Delivery_ID = "3", Delivery_Weight = 35.00, Job_ID = "2", Driver_ID = "1", TruckID = "1", TrailerID = "1"
                },
                new Delivery
                {
                    Delivery_ID = "4", Delivery_Weight = 35.00, Job_ID = "2", Driver_ID = "1", TruckID = "1", TrailerID = "1"
                }
            );


            modelBuilder.Entity<Driver>().HasData(
                new Driver { Driver_ID = "1", Email = "Driver1@gmail.com" ,Name = "Driver1", Lastname = "Koen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "2", Email = "Driver2@gmail.com" , Name = "Driver2", Lastname = "Poen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "3", Email = "Driver3@gmail.com" , Name = "Driver3", Lastname = "Soen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "4", Email = "Driver4@gmail.com", Name = "Driver4", Lastname = "Loen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "5", Email = "Driver5@gmail.com", Name = "Driver5", Lastname = "Hoen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "6", Email = "Driver6@gmail.com", Name = "Driver6", Lastname = "Joen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "7", Email = "Driver7@gmail.com", Name = "Driver7", Lastname = "Doen", PhoneNumber = "0761532265", Driver_Status_ID = "1" },
                new Driver { Driver_ID = "8", Email = "Driver8@gmail.com", Name = "Driver8", Lastname = "Roen", PhoneNumber = "0761532265", Driver_Status_ID = "1" }
            );

            modelBuilder.Entity<Truck>().HasData(
                new Truck { TruckID = "1", Truck_License = "GH39QP L", Model = "Mercedes",  Truck_Status_ID = "1" },
                new Truck { TruckID = "2", Truck_License = "AJ11LL L", Model = "Mercedes",  Truck_Status_ID = "1" },
                new Truck { TruckID = "3", Truck_License = "LL19AQ L", Model = "Mercedes",  Truck_Status_ID = "1" },
                new Truck { TruckID = "4", Truck_License = "TT11PP L", Model = "Mercedes", Truck_Status_ID = "1" },
                new Truck { TruckID = "5", Truck_License = "QW12ER L", Model = "Mercedes", Truck_Status_ID = "1" }
            );

            modelBuilder.Entity<Trailer>().HasData(
                new Trailer { TrailerID = "1", Trailer_License = "PO69EN L", Model = "Palumbo", Weight = 35, Trailer_Type_ID = "1" ,Trailer_Status_ID = "1" },
                new Trailer { TrailerID = "2", Trailer_License = "EH42ML L", Model = "Palumbo", Weight = 35, Trailer_Type_ID = "1", Trailer_Status_ID = "1" },
                new Trailer { TrailerID = "3", Trailer_License = "PQ11LE L", Model = "Palumbo", Weight = 35, Trailer_Type_ID = "1", Trailer_Status_ID = "1" },
                new Trailer { TrailerID = "4", Trailer_License = "HJ91LO L", Model = "Palumbo", Weight = 35, Trailer_Type_ID = "1", Trailer_Status_ID = "1" },
                new Trailer { TrailerID = "5", Trailer_License = "AS99BN L", Model = "Palumbo", Weight = 35, Trailer_Type_ID = "1", Trailer_Status_ID = "1" }
            );

            modelBuilder.Entity<ProductType>().HasData(
                new ProductType { Product_Type_ID = "1", Name = "Truck", Description = "Product has trailer components" },
                new ProductType { Product_Type_ID = "2", Name = "Trailer", Description = "Product has truck components" }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Product_Category_ID = "1", Name = "Engine", Description = "products for engines"},
                new ProductCategory { Product_Category_ID = "2", Name = "Transmission", Description = "products for transmissions" },
                new ProductCategory { Product_Category_ID = "3", Name = "Suspension", Description = "products for suspensions"},
                new ProductCategory { Product_Category_ID = "4", Name = "Electrical", Description = "products for electrical" },
                new ProductCategory { Product_Category_ID = "5", Name = "Body", Description = "products for body"},
                new ProductCategory { Product_Category_ID = "6", Name = "Brake", Description = "products for brakes"},
                new ProductCategory { Product_Category_ID = "7", Name = "Wheel", Description = "products for wheels"},
                new ProductCategory { Product_Category_ID = "8", Name = "Consumables", Description = "bolts,nuts ect.." }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Product_ID = "1", Product_Name = "Fuel Pump", Product_Description = "FUEL PRIMER PUMP/K5", Product_Price = 999, Quantity = 10, Product_Category_ID = "4", Product_Type_ID = "2" },
                new Product { Product_ID = "2", Product_Name = "SEAL RING", Product_Description = "SEAL RING MB-S48", Product_Price = 899, Quantity = 9, Product_Category_ID = "5", Product_Type_ID = "1" },
                new Product { Product_ID = "3", Product_Name = "CLUTCH", Product_Description = "CLUTCH MASTER CYL 24mm SIDE MOUNT-S10", Product_Price = 1499, Quantity = 7, Product_Category_ID = "7", Product_Type_ID = "2" },
                new Product { Product_ID = "4", Product_Name = "AXLE NUT", Product_Description = "SAF AXLE NUT LEFT M75x1.5 (85mm)", Product_Price = 1199, Quantity = 66, Product_Category_ID = "7", Product_Type_ID = "1" },
                new Product { Product_ID = "5", Product_Name = "BEARING", Product_Description = "BEARING INN ROCKWELL TM 218248/210/HM", Product_Price = 9.99, Quantity = 23, Product_Category_ID = "8", Product_Type_ID = "1" },
                new Product { Product_ID = "6", Product_Name = "SEAL OIL", Product_Description = "SEAL OIL STEERING M/B AXOR-S46", Product_Price = 119.99, Quantity = 40, Product_Category_ID = "6", Product_Type_ID = "1" },
                new Product { Product_ID = "7", Product_Name = "BRAKEPAD", Product_Description = "BRAKEPAD TO FIT MAN TGS/TGX WVA29279", Product_Price = 799, Quantity = 5, Product_Category_ID = "7", Product_Type_ID = "1" },
                new Product { Product_ID = "8", Product_Name = "FAN BELT", Product_Description = "FAN BELT 9PK2300-U7", Product_Price = 455, Quantity = 7, Product_Category_ID = "1", Product_Type_ID = "1"}
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Order_ID = "1", Date = DateTime.Now, Status="Ordered", Total = 2897, Customer_ID = "1" },
                new Order { Order_ID = "2", Date = DateTime.Now, Status = "Ordered", Total = 2997, Customer_ID = "2" },
                new Order { Order_ID = "3", Date = DateTime.Now, Status = "Ordered", Total = 2998, Customer_ID = "3" }
            );

            modelBuilder.Entity<Order_Line>().HasData(
               new Order_Line { Order_line_ID = "8", Orderid = "1", Productid = "1", Quantity = 2, SubTotal = 1998 },
               new Order_Line { Order_line_ID = "8", Orderid = "1", Productid = "2", Quantity = 1, SubTotal = 899 },
               new Order_Line { Order_line_ID = "8", Orderid = "2", Productid = "1", Quantity = 3, SubTotal = 2997 },
               new Order_Line { Order_line_ID = "8", Orderid = "3", Productid = "3", Quantity = 2, SubTotal = 2998 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Customer_ID = "1", Name = "John", LastName = "Doe", Email = "johndoe@gmail.com", Password = "john123" },
                new Customer { Customer_ID = "2", Name = "Jane", LastName = "Smith", Email = "janesmith@gmail.com", Password = "jane123" },
                new Customer { Customer_ID = "3", Name = "Joe", LastName = "Mama", Email = "joemama@gmail.com", Password = "joe123" }
            );

            modelBuilder.Entity<Invoice>().HasData(
                new Invoice { Invoice_number = "1", Order_ID = "1", Total_Amount = 200.50, Date = DateTime.Now },
                new Invoice { Invoice_number = "2", Order_ID = "2", Total_Amount = 75.20, Date = DateTime.Now },
                new Invoice { Invoice_number = "3", Order_ID = "3", Total_Amount = 450.00, Date = DateTime.Now }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment { Payment_ID = "1", Order_ID = "1", Payment_Type_ID = "1", amount_paid = 150.50, Date = DateTime.Now },
                new Payment { Payment_ID = "2", Order_ID = "1", Payment_Type_ID = "2", amount_paid = 50.00, Date = DateTime.Now },
                new Payment { Payment_ID = "3", Order_ID = "2", Payment_Type_ID = "3", amount_paid = 75.20, Date = DateTime.Now },
                new Payment { Payment_ID = "4", Order_ID = "3", Payment_Type_ID = "1", amount_paid = 200.00, Date = DateTime.Now },
                new Payment { Payment_ID = "5", Order_ID = "3", Payment_Type_ID = "2", amount_paid = 250.00, Date = DateTime.Now }
            );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { Payment_Type_ID = "1", Name = "Credit Card", Descrtipion = "Customer paid with credit card" },
                new PaymentType { Payment_Type_ID = "2", Name = "EFT", Descrtipion = "Customer paid with EFT" },
                new PaymentType { Payment_Type_ID = "3", Name = "Cash", Descrtipion = "Customer paid with cash" }
            );

            modelBuilder.Entity<Admin>().HasData(
                new Admin { Admin_ID = "1", Name = "Hanru", Lastname = "du Plessis", Email = "hanruduplessis@gmail.com", Password = "hanru123" },
                new Admin { Admin_ID = "2", Name = "admin", Lastname="admin", Email="admin@gmail.com",Password="admin123" }
            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Supplier_ID = "1", Name = "ABC Suppliers", Email = "abc@gmail.com", Contact_Number = "0125554789" },
                new Supplier { Supplier_ID = "2", Name = "XYZ Suppliers", Email = "xyz@gmail.com", Contact_Number = "0125554789" }
            );

            modelBuilder.Entity<Product_Supplier>().HasData(
                new Product_Supplier { Product_Supplier_ID = "1", Productid = "1", Supplierid = "1" },
                new Product_Supplier { Product_Supplier_ID = "2", Productid = "2", Supplierid = "1" },
                new Product_Supplier { Product_Supplier_ID = "3", Productid = "2", Supplierid = "2" },
                new Product_Supplier { Product_Supplier_ID = "4", Productid = "3", Supplierid = "2" }
            );

            modelBuilder.Entity<TruckStatus>().HasData(
                new TruckStatus { Truck_Status_ID = "1", Status = "Available", Description = "Truck is available for job" },
                new TruckStatus { Truck_Status_ID = "2", Status = "Unavailable", Description = "Truck is busy with a job" },
                new TruckStatus { Truck_Status_ID = "3", Status = "Under Maintenance", Description = "Truck is undergoing maintenace" }
            );
            modelBuilder.Entity<TrailerStatus>().HasData(
                new TrailerStatus { Trailer_Status_ID = "1", Status = "Available", Description = "Trailer is available for job" },
                new TrailerStatus { Trailer_Status_ID = "2", Status = "Unavailable", Description = "Trailer is busy with a job" },
                new TrailerStatus { Trailer_Status_ID = "3", Status = "Under Maintenance", Description = "Trailer is undergoing maintenace" }
            );
            modelBuilder.Entity<TrailerType>().HasData(
                new TrailerType { Trailer_Type_ID = "1", Name = "Coal", Description = "Coal transportation trailer" }
            );

            modelBuilder.Entity<DriverStatus>().HasData(
                new DriverStatus { Driver_Status_ID = "1", Status = "Available", Description = "Driver is available" },
                new DriverStatus { Driver_Status_ID = "2", Status = "Unavailable", Description = "Driver is busy with a job" },
                new DriverStatus { Driver_Status_ID = "3", Status = "Busy", Description = "Driver is unable to do a job" }
            );

        }
    }
}
 
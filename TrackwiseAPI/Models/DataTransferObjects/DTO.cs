﻿using System.ComponentModel.DataAnnotations.Schema;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.DataTransferObjects
{
    public class OrderDTO
    {
        public string Order_ID { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public string Customer_ID { get; set; }
        public CustomerDTO? Customer { get; set; }
        public ICollection<OrderLineDTO> OrderLines { get; set; }
    }

    public class OrderLineDTO
    {
        public string Order_line_ID { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
    }
    public class OrderLineInvoiceDTO
    {
        public string Order_line_ID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
    }

    public class CustomerDTO
    {
        public string Customer_ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class ProductDTO
    {
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public double Product_Price { get; set; }
        public int Quantity { get; set; }
        public bool? ListStatus { get; set; }
        public string? Image { get; set; }
        public ProductTypeDTO Product_Type { get; set; }
        public ProductCategoryDTO Product_Category { get; set; }
    }

    public class ProductTypeDTO
    {
        public string Product_Type_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductCategoryDTO
    {
        public string Product_Category_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductSpesificTypeDTO
    {
        public string Product_Type_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductSpesificCategoryDTO Product_Category { get; set; }
    }

    public class ProductSpesificCategoryDTO
    {
        public string Product_Category_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductSpesificTypeDTO Product_Type { get; set; }
    }

    public class DeliveryDTO
    {
        public string Delivery_ID { get; set; }
        public double Delivery_Weight { get; set; }
        public string Driver_ID { get; set; }
        public string TrailerID { get; set; }
        public string TruckID { get; set; }
        public string Delivery_Status_ID { get; set; }
        public JobDTO Jobs { get; set; }
        public ICollection<DocumentDTO> Documents { get; set; }
    }
    public class JobDTO
    {
        public string Job_ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string type { get; set; }
        public string mapURL { get; set; }
        // Add other properties from Job that you want to include in the DTO
    }

    public class DocumentDTO
    {
        public string Document_ID { get; set; }
        public string Image { get; set; }
        public string Delivery_ID { get; set; }
        public string DocType { get; set; }
    }

    public class CompleteJobsDTO
    {
        public string Job_ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double Weight { get; set; }
        public string Creator_ID { get; set; }
        public string type { get; set; }
        public string Job_Status_ID { get; set; }
        public JobStatusDTO JobStatus { get; set; }
        public string Delivery_ID { get; set; }
        public Delivery Deliveries { get; set; }
        public string TruckID { get; set; }
        public Truck Truck { get; set; }
    }

    public class JobStatusDTO
    {
        public string Job_Status_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class LoadsCarriedDTO
    {
        public string Registration { get; set; }
        public int Trip { get; set; }
        public double Weight { get; set; }
    }


    public class SalesDTO
    {
        public string Product_Name { get; set; }
        public string Product_Category { get; set; }
        public string Product_Type { get; set; }
        public int Quantity_Sold { get; set; }
        public int Price_Per_Quantity { get; set; }
        public int Total { get; set; }
    }


    public class MileageFuelDTO
    {
        public string Delivery_ID { get; set; }
        public double? Mileage { get; set; }
        public double? Fuel { get; set; }
        //public double? Total_Mileage { get; set; }
        //public double? Total_Fuel { get; set; }

    }

    public class TruckDataDTO
    {
        public string Registration { get; set; }
        public List<MileageFuelDTO> MFList { get; set; } // Use a list to store multiple TruckMFDTO objects

    }

    public class TotalSalesDTO
    {
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }

    public class JobListingDTO
    {
        public string Job_ID { get; set; }
        public double Weight { get; set; }
        public string Creator { get; set; }
        public int Trips { get; set; }
    }

    public class AdminDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }

    public class DriverDTO
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class JobDetailDTO
    {
        public string Job_ID { get; set; }
        public double Total_Weight { get; set; }
        public int Total_Trips { get; set; }
        public string Pickup_Location { get; set; }
        public string Dropoff_Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public List<deliveryDetailDTO> deliveryList { get; set; }
    }
    public class deliveryDetailDTO
    {
        public string Delivery_ID { get; set; }
        public double Delivery_Weight { get; set; }
        public int Trips { get; set; }
        
    }

}

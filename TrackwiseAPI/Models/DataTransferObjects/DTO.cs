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
    public class DeliveryDTO
    {
        public string Delivery_ID { get; set; }
        public double Delivery_Weight { get; set; }
        public string Driver_ID { get; set; }
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
    }
}

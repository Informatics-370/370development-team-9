namespace TrackwiseAPI.Models.DataTransferObjects
{
    public class OrderDTO
    {
        public string Order_ID { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public string Customer_ID { get; set; }
        public ICollection<OrderLineDTO> OrderLines { get; set; }
    }

    public class OrderLineDTO
    {
        public string Order_line_ID { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
    }

    public class ProductDTO
    {
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public double Product_Price { get; set; }
        public int Quantity { get; set; }
        public string Product_Type_ID { get; set; }
        public string Product_Category_ID { get; set; }
    }

}

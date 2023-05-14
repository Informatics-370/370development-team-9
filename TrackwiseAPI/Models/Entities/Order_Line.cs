using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Order_Line
    {
        [Key]
        public int Order_line_ID { get; set; }
        public int Orderid { get; set; }
        public Order Order { get; set; }

        public int Productid { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double SubTotal { get; set; }
    }
}

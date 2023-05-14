using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Invoice
    {
        [Key]
        public int Invoice_number { get; set; }
        public double Total_Amount { get; set; }
        public DateTime Date { get; set; }

        //Foreign Key for Order_ID
        public int Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public Order Order { get; set; }
    }
}

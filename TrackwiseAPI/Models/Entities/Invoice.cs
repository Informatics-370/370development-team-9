using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Invoice
    {
        [Key]
        public string Invoice_number { get; set; }
        public double Total_Amount { get; set; }
        public DateTime Date { get; set; }

        //Foreign Key for Order_ID
        public string Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public Order Order { get; set; }
    }
}

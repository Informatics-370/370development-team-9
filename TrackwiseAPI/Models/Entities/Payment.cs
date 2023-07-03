using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Payment
    {
        [Key]
        public string Payment_ID { get; set; }
        public DateTime Date { get; set; }
        public double amount_paid { get; set; }

        //Foreign Key for Order_ID
        public string Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public Order Order { get; set; }

        //Foreign Key for Order_ID
        public string Payment_Type_ID { get; set; }
        [ForeignKey("Payment_Type_ID")]
        public PaymentType PaymentType { get; set; }
    }
}

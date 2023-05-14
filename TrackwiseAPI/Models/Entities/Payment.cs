using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Payment
    {
        [Key]
        public int Payment_ID { get; set; }
        public DateTime Date { get; set; }
        public double amount_paid { get; set; }

        //Foreign Key for Order_ID
        public int Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public Order Order { get; set; }

        //Foreign Key for Order_ID
        public int Payment_Type_ID { get; set; }
        [ForeignKey("Payment_Type_ID")]
        public PaymentType PaymentType { get; set; }
    }
}

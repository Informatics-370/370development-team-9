using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Models.Entities
{
    public class Order
    {
        [Key]
        public string Order_ID { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }

        //Foreign Key for Customer_ID
        public string Customer_ID { get; set; }
        [ForeignKey("Customer_ID")]
        public Customer Customer { get; set; }

        public ICollection<Payment> payments { get; set; }
        public ICollection<Order_Line> OrderLines { get; set; }
         
    }

    public class CheckoutRequest
    {
        public OrderVM orderVM { get; set; }
        public NewCard newCard { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class Delivery_Assignment
    {
        [Key]
        public string Delivery_Assignment_ID { get; set; }
        public DateTime Date { get; set; }

        public string Deliveryid { get; set; }
        public Delivery Delivery { get; set; }

        public string Driverid { get; set; }
        public Driver Driver { get; set; }
    }
}

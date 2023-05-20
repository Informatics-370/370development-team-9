using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class Delivery_Assignment
    {
        [Key]
        public int Delivery_Assignment_ID { get; set; }
        public DateTime Date { get; set; }

        public int Deliveryid { get; set; }
        public Delivery Delivery { get; set; }

        public int Driverid { get; set; }
        public Driver Driver { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class DeliveryStatus
    {
        [Key]
        public string Delivery_Status_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }
}

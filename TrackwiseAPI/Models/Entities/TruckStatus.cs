using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class TruckStatus
    {
        [Key]
        public int Truck_Status_ID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ICollection<Truck> Trucks { get; set; }
    }
}

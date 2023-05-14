using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Truck
    {
        [Key]
        public int Truck_License { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }

        //Foreign Key for truck status
        public int Truck_Status_ID { get; set; }
        [ForeignKey("Truck_Status_ID")]
        public TruckStatus TruckStatus { get; set; }

        public ICollection<Trip_Truck> Trip_Trucks { get; set; }
    }
}

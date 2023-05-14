using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Trip_Truck
    {
        [Key]
        public int triptruck_id { get; set; }
        public int Truckid { get; set; }
        public int Tripid { get; set; }
        public Trip Trip { get; set; }
        public Truck Truck { get; set; }
        public string description { get; set; }

    }
}

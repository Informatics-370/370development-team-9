using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Trip
    {
        [Key]
        public int Trip_ID { get; set; }
        public double initialMileage { get; set; }
        public double FinalMileage { get; set; }
        public double Feul_input { get; set; }
        public double Feul_consumed { get; set; }
        public ICollection<Trip_Truck> Trip_Trucks { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class DriverStatus
    {
        [Key]
        public int Driver_Status_ID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}

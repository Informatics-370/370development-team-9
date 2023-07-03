using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class DriverStatus
    {
        [Key]
        public string Driver_Status_ID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}

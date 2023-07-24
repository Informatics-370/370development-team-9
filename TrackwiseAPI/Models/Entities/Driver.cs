using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Driver
    {
        [Key]
        public string Driver_ID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //Foreign Key for Driver Status
        public string Driver_Status_ID { get; set; }
        [ForeignKey("Driver_Status_ID")]
        public DriverStatus DriverStatus { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

    }
}

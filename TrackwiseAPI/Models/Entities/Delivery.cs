using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Delivery
    {
        [Key]
        public string Delivery_ID { get; set; }
        public double Delivery_Weight { get; set; }

        //Foreign Key for Job
        public string  Job_ID { get; set; }
        [ForeignKey("Job_ID")]
        public Job Job { get; set; }

        //Foreign Key for Driver
        public string Driver_ID { get; set; }
        [ForeignKey("Driver_ID")]
        public Driver Driver { get; set; }

        //Foreign Key for Truck
        public string TruckID { get; set; }
        [ForeignKey("TruckID")]
        public Truck Truck { get; set; }

        //Foreign Key for Trailer
        public string TrailerID { get; set; }
        [ForeignKey("TrailerID")]
        public Trailer Trailer { get; set; }

        public ICollection<Document> Documents { get; set; }

    }
}

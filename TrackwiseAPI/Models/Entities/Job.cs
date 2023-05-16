using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Job
    {
        [Key]
        public string Job_ID { get; set; }
        public DateTime Date { get; set; }
        public string Pickup_Location { get; set; }
        public string Dropoff_Location { get; set; }
        public double Weight { get; set; }

        //Foreign key for Client
        public int Client_ID { get; set; }
        [ForeignKey("Client_ID")]
        public Client Client { get; set; }

        //Foreign key for Admin
        public int Admin_ID { get; set; }
        [ForeignKey("Admin_ID")]
        public Admin Admin { get; set; }

        //Foreign key for Job_Type
        public int Job_Type_ID { get; set; }
        [ForeignKey("Job_Type_ID")]
        public JobType JobType { get; set; }

        //Foreign key for Job_Status
        public int Job_Status_ID { get; set; }
        [ForeignKey("Job_Status_ID")]
        public JobStatus JobStatus { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

    }
}

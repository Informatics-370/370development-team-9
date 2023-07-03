using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Delivery
    {
        [Key]
        public string Delivery_ID { get; set; }
        public double weight { get; set; }

        //Foreign Key for Job
        public string Job_ID { get; set; }
        [ForeignKey("Job_ID")]
        public Job Job { get; set; }

        //Foreign Key for Truck
    /*    public string Truck_License { get; set; }
        [ForeignKey("Truck_License")]
        public Truck Truck { get; set; }*/

        public ICollection<Delivery_Assignment> Delivery_Assignments { get; set; }

    }
}

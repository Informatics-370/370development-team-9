﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Job
    {
        [Key]
        public string Job_ID { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        
        public string Pickup_Location { get; set; }
        public string Dropoff_Location { get; set; }
        public double Total_Weight { get; set; }
        public string Creator_ID { get; set; }
        public string? Map { get;set; }
        public int? DeliveryCount { get; set; }
        public int CompletedDeliveries { get; set; }

        //Foreign key for Job_Type
        public string Job_Type_ID { get; set; }
        [ForeignKey("Job_Type_ID")]
        public JobType JobType { get; set; }

        //Foreign key for Job_Status
        public string Job_Status_ID { get; set; }
        [ForeignKey("Job_Status_ID")]
        public JobStatus JobStatus { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Truck
    {
        [Key]
        public string TruckID { get; set; }
        public string Truck_License { get; set; }
        public string Model { get; set; }
        public double? Mileage { get; set; }

        //Foreign Key for truck status
        public string Truck_Status_ID { get; set; }
        [ForeignKey("Truck_Status_ID")]
        public TruckStatus TruckStatus { get; set; }

        //Foreign Key for Driver
        /*  public int Driver_ID { get; set; }
          [ForeignKey("Driver_ID")]
          public Driver Driver { get; set; }

          //Foreign Key for Trailer
          public string? Trailer_License { get; set; }
          [ForeignKey("Trailer_License ")]
          public Trailer Trailer { get; set; }*/

        public ICollection<Delivery> Deliveries { get; set; }
    }
}

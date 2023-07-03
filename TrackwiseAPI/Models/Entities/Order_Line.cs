﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Order_Line
    {
        [Key]
        public string Order_line_ID { get; set; }
        public string Orderid { get; set; }
        public Order Order { get; set; }

        public string Productid { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double SubTotal { get; set; }
    }
}

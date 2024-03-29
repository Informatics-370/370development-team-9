﻿using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class TrailerType
    {
        [Key]
        public string Trailer_Type_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Trailer> Trailers { get; set; }
    }
}

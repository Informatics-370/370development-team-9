using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class TrailerStatus
    {
        [Key]
        public int Trailer_Status_ID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public ICollection<Trailer> Trailers { get; set; }
    }
}

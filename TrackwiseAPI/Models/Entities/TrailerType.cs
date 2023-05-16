using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class TrailerType
    {
        [Key]
        public int Trailer_Type_ID { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public ICollection<Trailer> Trailers { get; set; }
    }
}

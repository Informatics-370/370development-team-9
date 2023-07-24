using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Trailer
    {
        [Key]
        public string TrailerID { get; set; }
        public string Trailer_License { get; set; }
        public string Model { get; set; }
        public double Weight { get; set; }

        //Foreign Key for Trailer Type
        public string Trailer_Type_ID { get; set; }
        [ForeignKey("Trailer_Type_ID")]
        public TrailerType TrailerType { get; set; }

        //Foreign Key for Trailer Status
        public string Trailer_Status_ID { get; set; }
        [ForeignKey("Trailer_Status_ID")]
        public TrailerStatus TrailerStatus { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }
    }
}

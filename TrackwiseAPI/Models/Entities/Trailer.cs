using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Trailer
    {
        [Key]
        public int Trailer_License { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public double Total_Trips { get; set; }

        //Foreign Key for Trailer Type
        public int Trailer_Type_ID { get; set; }
        [ForeignKey("Trailer_Type_ID")]
        public TrailerType TrailerType { get; set; }

        //Foreign Key for Trailer Status
        public int Trailer_Status_ID { get; set; }
        [ForeignKey("Trailer_Type_ID")]
        public TrailerStatus TrailerStatus { get; set; }
    }
}

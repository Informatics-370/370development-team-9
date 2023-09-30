using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class MaxHrs
    {
        [Key]
        public string Hrs_ID { get; set; }
        public double Hrs_Amount { get; set; }
    }
}

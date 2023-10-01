using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class RestPeriod
    {
        [Key]
        public string Rest_ID { get; set; }
        public double Rest_Amount { get; set; }
    }
}

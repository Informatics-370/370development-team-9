using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class JobRule
    {
        [Key]
        public string Rule_ID { get; set; }
        public double Break { get; set; }
        public double Rest { get; set; }
        public double MaxHrs { get; set; }
    }
}

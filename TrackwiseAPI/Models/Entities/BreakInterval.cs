using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class BreakInterval
    {
        [Key]
        public string Break_ID { get; set; }
        public double Break_Amount { get; set; }
    }
}

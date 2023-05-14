using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class HelpCategory
    {
        [Key]
        public int Help_Category_ID { get; set; }
        public string Description { get; set; }
        public ICollection<Help> Helps { get; set; }
    }
}

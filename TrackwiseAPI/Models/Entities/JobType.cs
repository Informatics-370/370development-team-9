using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class JobType
    {
        [Key]
        public string Job_Type_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Job> jobs { get; set; }
    }
}

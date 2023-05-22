using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class JobType
    {
        [Key]
        public int Job_Type_ID { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public ICollection<Job> jobs { get; set; }
    }
}

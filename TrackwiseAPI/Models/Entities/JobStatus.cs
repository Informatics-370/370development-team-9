using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class JobStatus
    {
        [Key]
        public string Job_Status_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Job> Jobs { get; set; }

    }
}

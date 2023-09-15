using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class Audit
    {
        [Key]
        public string Audit_ID { get; set; }
        public string User { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Action { get; set;}
    }
}

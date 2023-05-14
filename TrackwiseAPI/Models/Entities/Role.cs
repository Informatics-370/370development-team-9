using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class Role
    {
        [Key]
        public int Role_ID { get; set; }
        public string RoleName { get; set; }    
    }
}

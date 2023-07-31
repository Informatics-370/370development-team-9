using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class Role
    {
        [Key]
        public string Role_ID { get; set; }
        public string RoleName { get; set; }    
    }
}

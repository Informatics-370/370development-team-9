using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Client
    {
        [Key]
        public string Client_ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        /*       //Foreign key for User_ID
                public int User_ID { get; set; }
                [ForeignKey("User_ID")]
                public User User { get; set; }*/
        public ICollection<Job> jobs { get; set; }
    }
}

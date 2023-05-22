using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Admin
    {
        [Key]
        public int Admin_ID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //Foreign key for User_ID
        //public int User_ID { get; set; }
        //[ForeignKey("User_ID")]
        //public User User { get; set; }

        public ICollection<Job> jobs { get; set; }
    }
}

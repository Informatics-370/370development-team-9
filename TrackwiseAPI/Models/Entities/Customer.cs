using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Customer
    {
        [Key]
        public int Customer_ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //Foreign key for User_ID
/*        public int User_ID { get; set; }
        [ForeignKey("User_ID")]
        public User User { get; set; }*/

        public ICollection<Order> orders { get; set; }
    }
}

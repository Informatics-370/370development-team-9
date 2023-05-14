using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        /*
        public string User_Email { get; set;}   
        public string User_Password { get; set;}

        //Foreign key for Role_ID
        public int Role_ID { get; set; }
        [ForeignKey("Role_ID")]
        public Role Role { get; set; }

        public ICollection<Admin> Admins { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<Driver> Drivers { get; set; }*/

    }
}

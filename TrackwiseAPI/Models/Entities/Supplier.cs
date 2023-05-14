using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Supplier
    {
        [Key]
        public int Supplier_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        //Foreign key for Admin_ID
        public int Admin_ID { get; set; }
        [ForeignKey("Admin_ID")]
        public Admin Admin { get; set; }

        public ICollection<Product_Supplier> Product_Suppliers { get; set; }
    }
}

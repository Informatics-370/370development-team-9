using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Supplier
    {
        [Key]
        public string Supplier_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact_Number { get; set; }

        public ICollection<Product_Supplier> Product_Suppliers { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Product_Supplier
    {
        [Key]
        public int Product_Supplier_ID { get; set; }

        public int Supplierid { get; set; }
        public Supplier Supplier { get; set; }

        public int Productid { get; set; }
        public Product Product { get; set; }

    }
}

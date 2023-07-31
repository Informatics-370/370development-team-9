using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Product_Supplier
    {
        [Key]
        public string Product_Supplier_ID { get; set; }

        public string Supplierid { get; set; }
        public Supplier Supplier { get; set; }

        public string Productid { get; set; }
        public Product Product { get; set; }

    }
}

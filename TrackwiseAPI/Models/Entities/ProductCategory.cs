using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Product_Category_ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        //Foreign Key for Product_Type
        public int Product_Type_ID { get; set; }
        [ForeignKey("Product_Type_ID")]
        public ProductType ProductType { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

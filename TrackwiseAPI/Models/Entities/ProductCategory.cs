using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class ProductCategory
    {
        [Key]
        public string Product_Category_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductTypeCategories> ProductTypeCategories { get; set; }
    }
}

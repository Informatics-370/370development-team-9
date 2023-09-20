using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class ProductTypeCategories
    {
/*        [Key]
        public string ProductTypeCategories_ID { get; set; }*/
        public string Product_Category_ID { get; set; }
        [ForeignKey("Product_Category_ID")]
        public ProductCategory ProductCategory { get; set; }

        public string Product_Type_ID { get; set; }
        [ForeignKey("Product_Type_ID")]
        public ProductType ProductType { get; set; }
    }
}

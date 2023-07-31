using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class ProductType
    {
        [Key]
        public string Product_Type_ID { get; set; }
        public string Name { get;  set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

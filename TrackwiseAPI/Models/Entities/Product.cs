using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Product
    {
        [Key]
        public string Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public double Product_Price { get; set; }
        public int Quantity { get; set; }

        public Inventory Inventory { get; set; }


        //Foreign Key for Product_Category
        public string Product_Category_ID { get; set; }
        [ForeignKey("Product_Category_ID")]
        public ProductCategory ProductCategory { get; set; }

        //Foreign Key for Product_Type
        public string Product_Type_ID { get; set; }
        [ForeignKey("Product_Type_ID")]
        public ProductType ProductType { get; set; }

        public ICollection<Product_Supplier> Product_Suppliers { get; set; }
        public ICollection<Order_Line> OrderLines { get; set; }

    }

}

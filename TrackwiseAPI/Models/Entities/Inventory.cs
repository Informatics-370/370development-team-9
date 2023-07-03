using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Inventory
    {
        [Key]
        public string Id { get; set; }
        public string Product_ID { get; set; }
        public int quantity { get; set; }
        public int reorder_total { get; set; }
        public Product Product { get; set; }

    }
}

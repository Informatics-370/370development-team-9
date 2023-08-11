using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Document
    {
        [Key]
        public string Document_ID { get; set; }
        public string? Image { get; set; }
        public string DocType { get; set; }
        public string Delivery_ID { get; set; }
        [ForeignKey("Delivery_ID")]
        public Delivery Delivery { get; set; }
    }
}

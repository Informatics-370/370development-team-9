using System.ComponentModel.DataAnnotations;

namespace TrackwiseAPI.Models.Entities
{
    public class PaymentType
    {
        [Key]
        public string Payment_Type_ID { get; set; }
        public string Name { get; set; }
        public string Descrtipion { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}

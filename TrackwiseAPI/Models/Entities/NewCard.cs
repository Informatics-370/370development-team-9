using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Models.Entities
{
    public class NewCard
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public long CardNumber { get; set; }

        [Required]
        public uint CardExpiry { get; set; }

        [Required]
        public uint Cvv { get; set; }
        public bool Vault { get; set; }

        [Required]
        public decimal Amount { get; set; }

        //Foreign Key for Customer_ID
        public string? Order_ID { get; set; }
        [ForeignKey("Order_ID")]
        public OrderVM? Order { get; set; }
    }
}

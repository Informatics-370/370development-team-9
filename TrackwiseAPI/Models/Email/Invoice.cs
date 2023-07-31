using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Email
{
    public class Invoice
    {
        public string? Email { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public double? TotalAmount { get; set; }

    }
}

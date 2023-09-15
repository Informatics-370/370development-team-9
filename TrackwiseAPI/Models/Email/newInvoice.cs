using TrackwiseAPI.Models.DataTransferObjects;

namespace TrackwiseAPI.Models.Email
{
    public class newInvoice
    {
        public double? Total { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public List<OrderLineInvoiceDTO> OrderLines { get; set; }
    }
}

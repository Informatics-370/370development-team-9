using System.Collections.Generic;

namespace TrackwiseAPI.Models.ViewModels
{
    public class OrderVM
    {
        public List<OrderLineVM> OrderLines { get; set; }
    }

    public class OrderLineVM
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
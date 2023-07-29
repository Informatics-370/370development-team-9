using System.Collections.Generic;
using TrackwiseAPI.Models.Entities;

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
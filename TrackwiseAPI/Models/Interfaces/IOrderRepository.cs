using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task<bool> SaveChangesAsync();
        Task<Order[]> GetAllOrdersAsync();
        Task<Order> GetOrderAsync(string OrderID);
    }
}

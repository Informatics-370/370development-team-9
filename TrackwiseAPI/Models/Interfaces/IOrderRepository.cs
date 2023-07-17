using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task<bool> SaveChangesAsync();
        // Add other methods as needed (e.g., GetOrder, UpdateOrder, DeleteOrder, etc.)
    }
}

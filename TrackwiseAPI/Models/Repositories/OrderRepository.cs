using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TwDbContext _context;

        public OrderRepository(TwDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }


    // Implement other methods as needed (e.g., GetOrder, UpdateOrder, DeleteOrder, etc.)
}


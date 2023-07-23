using Microsoft.EntityFrameworkCore;
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

            public async Task<Order[]> GetAllOrdersAsync()
            {
            IQueryable<Order> query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                        .ThenInclude(p => p.ProductType)
                    .Include(o => o.OrderLines)
                        .ThenInclude(ol => ol.Product)
                            .ThenInclude(p => p.ProductCategory);
            return await query.ToArrayAsync();
            }

        public async Task<Order> GetOrderAsync(string OrderID)
        {
            IQueryable<Order> query = _context.Orders.Where(c => c.Order_ID == OrderID);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Order[]> GetAllCustomerOrdersAsync(string customerId)
        {
            IQueryable<Order> query = _context.Orders
                .Where(c => c.Customer_ID == customerId)
                .Include(o => o.Customer)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                        .ThenInclude(p => p.ProductType)
                    .Include(o => o.OrderLines)
                        .ThenInclude(ol => ol.Product)
                            .ThenInclude(p => p.ProductCategory);
            return await query.ToArrayAsync();
        }
    }


   
}


using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TwDbContext _context;

        public CustomerRepository(TwDbContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<Customer[]> GetAllCustomerAsync()
        {
            IQueryable<Customer> query = _context.Customers.Include(c => c.orders).ThenInclude(c => c.payments);
            return await query.ToArrayAsync();
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            IQueryable<Customer> query = _context.Customers.Where(c => c.Customer_ID == customerId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

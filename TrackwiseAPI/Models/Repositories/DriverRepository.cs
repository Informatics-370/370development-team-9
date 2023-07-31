using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly TwDbContext _context;

        public DriverRepository(TwDbContext context)
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
        public async Task<Driver[]> GetAllDriversAsync()
        {
            IQueryable<Driver> query = _context.Drivers.Include(t => t.DriverStatus);
            return await query.ToArrayAsync();
        }

        public async Task<Driver> GetDriverAsync(string Driver_ID)
        {
            IQueryable<Driver> query = _context.Drivers.Where(t => t.Driver_ID == Driver_ID).Include(t => t.DriverStatus);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

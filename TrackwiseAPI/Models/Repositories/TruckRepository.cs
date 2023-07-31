using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class TruckRepository: ITruckRepository
    {
        private readonly TwDbContext _context;

        public TruckRepository(TwDbContext context)
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
        public async Task<Truck[]> GetAllTrucksAsync()
        {
            IQueryable<Truck> query = _context.Trucks.Include(t => t.TruckStatus);
            return await query.ToArrayAsync();
        }

        public async Task<Truck> GetTruckAsync(string TruckID)
        {
            IQueryable<Truck> query = _context.Trucks.Where(t => t.TruckID == TruckID).Include(t => t.TruckStatus);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

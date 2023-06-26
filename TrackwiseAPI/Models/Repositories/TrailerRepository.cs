using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class TrailerRepository: ITrailerRepository
    {
        private readonly TwDbContext _context;

        public TrailerRepository(TwDbContext context)
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
        public async Task<Trailer[]> GetAllTrailerAsync()
        {
            IQueryable<Trailer> query = _context.Trailers.Include(c=>c.TrailerType).Include(c=> c.TrailerStatus);
            return await query.ToArrayAsync();
        }

        public async Task<Trailer> GetTrailerAsync(int TrailerID)
        {
            IQueryable<Trailer> query = _context.Trailers.Where(c => c.TrailerID == TrailerID).Include(c=> c.TrailerType).Include(c=>c.TrailerStatus);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

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
            IQueryable<Trailer> query = _context.Trailers/*.Include(c => c.orders).ThenInclude(c => c.payments)*/;
            return await query.ToArrayAsync();
        }

        public async Task<Trailer> GetTrailerAsync(string trailerLicense)
        {
            IQueryable<Trailer> query = _context.Trailers.Where(c => c.Trailer_License == trailerLicense);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

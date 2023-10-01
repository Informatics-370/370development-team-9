using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class BreakIntervalRepository : IBreakIntervalRepository
    {
        private readonly TwDbContext _context;

        public BreakIntervalRepository(TwDbContext context)
        {
            _context = context;
        }
        public void Update(BreakInterval breaks)
        {
            _context.breakIntervals.Update(breaks);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<BreakInterval> GetBreakAsync()
        {
            return await _context.breakIntervals.SingleOrDefaultAsync();
        }
    }
}

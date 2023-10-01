using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class HrsRepository : IHrsRepository
    {
        private readonly TwDbContext _context;

        public HrsRepository(TwDbContext context)
        {
            _context = context;
        }
        public void Update(MaxHrs hours)
        {
            _context.MaxHrs.Update(hours);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MaxHrs> GetHrsAsync()
        {
            return await _context.MaxHrs.SingleOrDefaultAsync();
        }
    }
}

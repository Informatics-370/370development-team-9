using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class RestPeriodRepository : IRestPeriodRepository
    {
        private readonly TwDbContext _context;

        public RestPeriodRepository(TwDbContext context)
        {
            _context = context;
        }
        public void Update(RestPeriod rests)
        {
            _context.RestPeriods.Update(rests);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RestPeriod> GetRestAsync()
        {
            return await _context.RestPeriods.SingleOrDefaultAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class JobRuleRepository : IJobRuleRepository
    {
        private readonly TwDbContext _context;

        public JobRuleRepository(TwDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<JobRule> GetRuleAsync()
        {
            return await _context.Rule.SingleOrDefaultAsync();
        }
    }
}

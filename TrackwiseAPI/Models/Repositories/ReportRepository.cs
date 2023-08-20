using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly TwDbContext _context;
        public ReportRepository(TwDbContext context)
        {
            _context = context;
        }
        public async Task<Job[]> GetCompleteJobsAsync()
        {
            IQueryable<Job> query = _context.Jobs.Include(t => t.JobStatus).Include(t => t.JobType).Where(s => s.Job_Status_ID == "2");
            return await query.ToArrayAsync();
        }

        public async Task<Delivery[]> GetLoadsCarriedAsync()
        {
            IQueryable<Delivery> query = _context.Deliveries.Where(d => d.Delivery_Status_ID == "2");
            return await query.ToArrayAsync();
        }

        public async Task<Delivery[]> GetAllMileageFuelAsync()
        {
            IQueryable<Delivery> query = _context.Deliveries.Where(d => d.Delivery_Status_ID == "2");
            return await query.ToArrayAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TwDbContext _context;

        public AdminRepository(TwDbContext context)
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
        public async Task<Admin[]> GetAllAdminsAsync()
        {
            IQueryable<Admin> query = _context.Admins;
            return await query.ToArrayAsync();
        }

        public async Task<Admin> GetAdminAsync(int AdminID)
        {
            IQueryable<Admin> query = _context.Admins.Where(c => c.Admin_ID == AdminID);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}



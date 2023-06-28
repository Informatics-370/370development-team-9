using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly TwDbContext _context;

        public ClientRepository(TwDbContext context)
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
        public async Task<Client[]> GetAllClientsAsync()
        {
            IQueryable<Client> query = _context.Clients;
            return await query.ToArrayAsync();
        }

        public async Task<Client> GetClientAsync(string ClientID)
        {
            IQueryable<Client> query = _context.Clients.Where(c => c.Client_ID == ClientID);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}



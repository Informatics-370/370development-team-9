using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
       
            private readonly TwDbContext _context;

            public SupplierRepository(TwDbContext context)
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

            public async Task<Supplier[]> GetAllSuppliersAsync()
            {
                IQueryable<Supplier> query = _context.Suppliers.Include(c => c.Product_Suppliers);
                return await query.ToArrayAsync();
            }

            public async Task<Supplier> GetSupplierAsync(int supplierid)
            {
                IQueryable<Supplier> query = _context.Suppliers.Where(c => c.Supplier_ID == supplierid).Include(c => c.Product_Suppliers);
                return await query.FirstOrDefaultAsync();
            }

            public async Task<bool> SaveChangesAsync()
            {
                return await _context.SaveChangesAsync() > 0;
            }
        
    }
}

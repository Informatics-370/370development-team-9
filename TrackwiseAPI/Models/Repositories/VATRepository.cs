using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class VATRepository : IVATRepository
    {
        private readonly TwDbContext _context;

        public VATRepository(TwDbContext context)
        {
            _context = context;
        }
        public void Update(VAT vat)
        {
            _context.VAT.Update(vat);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<VAT> GetVATAsync()
        {
            return await _context.VAT.SingleOrDefaultAsync();
        }
    }
}

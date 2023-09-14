using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IVATRepository
    {
        void Update(VAT vat);
        Task<bool> SaveChangesAsync();
        Task<VAT> GetVATAsync();
    }
}

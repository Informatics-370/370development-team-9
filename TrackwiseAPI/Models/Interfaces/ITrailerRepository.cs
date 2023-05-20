using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface ITrailerRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Customer
        Task<Trailer[]> GetAllTrailerAsync();
        Task<Trailer> GetTrailerAsync(int TrailerID);
    }
}

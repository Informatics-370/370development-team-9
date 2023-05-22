using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IDriverRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Driver[]> GetAllDriversAsync();
        Task<Driver> GetDriverAsync(int Driver_ID);
    }
}

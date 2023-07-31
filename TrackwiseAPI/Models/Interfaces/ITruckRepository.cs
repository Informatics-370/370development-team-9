using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface ITruckRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Truck[]> GetAllTrucksAsync();
        Task<Truck> GetTruckAsync(string TruckID);
    }
}

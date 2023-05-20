using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IAdminRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Admin
        Task<Admin[]> GetAllAdminsAsync();
        Task<Admin> GetAdminAsync(int AdminID);
    }
}

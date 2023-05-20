using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IClientRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Client
        Task<Client[]> GetAllClientsAsync();
        Task<Client> GetClientAsync(int ClientID);
    }
}



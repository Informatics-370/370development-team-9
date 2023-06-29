using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface ICustomerRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Customer
        Task<Customer[]> GetAllCustomerAsync();
        Task<Customer> GetCustomerAsync(string customerId);
    }
}

using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IProductRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        void Update(Product product);

        //Supplier
        Task<Product[]> GetAllProductsAsync();
        Task<Product> GetProductAsync(string productId);
    }
}

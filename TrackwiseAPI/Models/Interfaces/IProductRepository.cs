using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IProductRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        void Update(Product product);
        ProductCategory FindByName(string categoryID);
        ProductType FindTypeByName(string TypeID);
        //Supplier
        Task<Product[]> GetAllProductsAsync();
        Task<Product> GetProductAsync(string productId);
        //Categories and type
        Task<ProductCategory[]> GetProductCategoryAsync();
        Task<ProductType[]> GetProductTypeAsync();
        Task<ProductTypeCategories[]> GetSpesificProductTypeAsync(string typeID);
        Task<ProductTypeCategories[]> GetSpesificProductCategoryAsync(string categoryID);
    }
}

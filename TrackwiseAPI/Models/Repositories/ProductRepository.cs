using Microsoft.EntityFrameworkCore;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;

namespace TrackwiseAPI.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TwDbContext _context;

        public ProductRepository(TwDbContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public ProductCategory FindByName(string categoryID)
        {
            // Assuming you have a ProductCategories DbSet in your DbContext
            return _context.ProductCategories.FirstOrDefault(pc => pc.Product_Category_ID == categoryID);
        }
        public ProductType FindTypeByName(string TypeID)
        {
            // Assuming you have a ProductCategories DbSet in your DbContext
            return _context.ProductTypes.FirstOrDefault(pc => pc.Product_Type_ID == TypeID);
        }

        public async Task<Product[]> GetAllProductsAsync()
        {
            IQueryable<Product> query = _context.Products.Include(c => c.ProductCategory).Include(c => c.ProductType);
            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(string productid)
        {
            IQueryable<Product> query = _context.Products.Where(c => c.Product_ID == productid).Include(c => c.ProductCategory).Include(c => c.ProductType);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProductType[]> GetProductTypeAsync()
        {
            IQueryable<ProductType> query = _context.ProductTypes;
            return await query.ToArrayAsync();
        }

        public async Task<ProductCategory[]> GetProductCategoryAsync()
        {
            IQueryable<ProductCategory> query = _context.ProductCategories;
            return await query.ToArrayAsync();
        }

        public async Task<ProductTypeCategories[]> GetSpesificProductTypeAsync(string typeID)
        {
            IQueryable<ProductTypeCategories> query = _context.ProductTypeCategories
                .Where(c => c.Product_Type_ID == typeID)
                .Include(o => o.ProductType)
                .Include(o => o.ProductCategory);
            return await query.ToArrayAsync();
        }

        public async Task<ProductTypeCategories[]> GetSpesificProductCategoryAsync(string categoryID)
        {
            IQueryable<ProductTypeCategories> query = _context.ProductTypeCategories
                .Where(c => c.Product_Category_ID == categoryID)
                .Include(o => o.ProductCategory)
                .Include(o => o.ProductType);
            return await query.ToArrayAsync();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Product[]> GetAllProductsAsync()
        {
            IQueryable<Product> query = _context.Products.Include(c => c.ProductCategory).ThenInclude(c => c.ProductType);
            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductAsync(int productid)
        {
            IQueryable<Product> query = _context.Products.Where(c => c.Product_ID == productid).Include(c => c.ProductCategory).ThenInclude(c => c.ProductType);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

﻿using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IProductRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Supplier
        Task<Product[]> GetAllProductsAsync();
        Task<Product> GetProductAsync(int productId);
    }
}
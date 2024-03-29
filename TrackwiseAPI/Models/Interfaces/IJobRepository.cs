﻿using Microsoft.EntityFrameworkCore.Storage;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IJobRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Job[]> GetAllAdminJobsAsync();
        Task<Job[]> GetAllClientJobsAsync(string clientId);
        Task<Driver[]> GetAvailableDriverAsync();
        Task<Trailer[]> GetAvailableTrailerWithTypeAsync(string id);
        Task<Truck[]> GetAvailableTruckAsync();
        IDbContextTransaction BeginTransaction();
        Task<Delivery[]> GetAllDeliveries();
        Task<DeliveryDTO[]> GetDriverDeliveriesAsync(string driverID);
        Task<Job> GetJobAsync(string Job_ID);
        Task<Delivery> GetDeliveryByID(string delivery_ID);
        Task<Document[]> GetDocumentsByDeliveryID(string deliveryID);
        Task<Delivery[]> GetAllMileageFuelAsync();

    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TrackwiseAPI.DBContext;
using TrackwiseAPI.Models.DataTransferObjects;
using TrackwiseAPI.Models.Entities;
using TrackwiseAPI.Models.Interfaces;
using TrackwiseAPI.Models.ViewModels;

namespace TrackwiseAPI.Models.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly TwDbContext _context;
        public JobRepository(TwDbContext context)
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
        public async Task<Job[]> GetAllAdminJobsAsync()
        {
            IQueryable<Job> query = _context.Jobs.Include(t => t.Deliveries).Include(t => t.JobStatus).Include(t => t.JobType);
            return await query.ToArrayAsync();
        }

        public async Task<Job[]> GetAllClientJobsAsync(string clientId)
        {
            IQueryable<Job> query = _context.Jobs.Where(c=> c.Creator_ID == clientId).Include(t => t.Deliveries).Include(t => t.JobStatus).Include(t => t.JobType);
            return await query.ToArrayAsync();
        }

        public async Task<Delivery[]> GetAllDeliveries()
        {
            IQueryable<Delivery> query = _context.Deliveries.Include(t => t.Job).Include(t => t.Driver).Include(t => t.Trailer).Include(t => t.Truck);
            return await query.ToArrayAsync();
        }

        //GET DRIVER AVAILABLE STATUS
        public async Task<Driver[]> GetAvailableDriverAsync()
        {
            IQueryable<Driver> query = _context.Drivers.Include(t => t.DriverStatus).Where(t => t.DriverStatus.Status == "Available");
            return await query.ToArrayAsync();
        }

        public async Task<DeliveryDTO[]> GetDriverDeliveriesAsync(string driverID)
        {
            var deliveries = await _context.Deliveries
                .Include(t => t.Driver)
                .Include(t => t.Trailer)
                .Include(t => t.Truck)
                .Include(t => t.DeliveryStatus)
                .Include(t => t.Job)
                    .ThenInclude(j => j.JobStatus)
                .Include(j => j.Job)
                    .ThenInclude(j => j.JobType)
                .Where(t => t.Driver_ID == driverID && t.Delivery_Status_ID == "1")
                .ToListAsync();

            // Map the entities to DTOs
            var deliveryDTOs = deliveries.Select(delivery => new DeliveryDTO
            {
                Delivery_ID = delivery.Delivery_ID,
                Delivery_Weight = delivery.Delivery_Weight,
                Driver_ID = delivery.Driver_ID,
                TrailerID = delivery.TrailerID,
                TruckID = delivery.TruckID,
                Delivery_Status_ID = delivery.Delivery_Status_ID,
                
                Jobs = new JobDTO
                {
                    Job_ID = delivery.Job.Job_ID,
                    StartDate = delivery.Job.StartDate,
                    DueDate = delivery.Job.DueDate,
                    PickupLocation = delivery.Job.Pickup_Location,
                    DropoffLocation = delivery.Job.Dropoff_Location,
                    type = delivery.Job.JobType.Name,
                    mapURL = delivery.Job.Map,
                }
            }).ToArray();

            return deliveryDTOs;
        }

        //Get Available Trailer and the matching type
        public async Task<Trailer[]> GetAvailableTrailerWithTypeAsync(string id)
        {
            IQueryable<Trailer> query = _context.Trailers.Include(t => t.TrailerStatus).Include(t => t.TrailerType)
                .Where(t => t.TrailerStatus.Status == "Available").Where(t => t.TrailerType.Trailer_Type_ID == id);
            return await query.ToArrayAsync();
        }

        //Get Trucks that are available
        public async Task<Truck[]> GetAvailableTruckAsync()
        {
            IQueryable<Truck> query = _context.Trucks.Include(t => t.TruckStatus).Where(t => t.TruckStatus.Status == "Available");
            return await query.ToArrayAsync();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        // Begin a new database transaction
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<Job> GetJobAsync(string Job_ID)
        {
            IQueryable<Job> query = _context.Jobs.Where(j => j.Job_ID == Job_ID)
                .Include(t => t.Deliveries)
                .ThenInclude(t => t.Truck)
                .Include(t => t.Deliveries)
                .ThenInclude(t => t.Trailer)
                .Include(t => t.Deliveries)
                .ThenInclude(d => d.Driver)
                .Include(t => t.JobStatus)
                .Include(t => t.JobType);
            return await query.FirstOrDefaultAsync();
        }
    }
}

using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IReportRepository
    {
        Task<Job[]> GetCompleteJobsAsync();
        Task<Delivery[]> GetLoadsCarriedAsync();
        //Task<Delivery[]> GetOrdersAsync();
        Task<Delivery[]> GetAllMileageFuelAsync();
        Task<Admin[]> GetAllAdminsAsync();
        Task<Driver[]> GetAllDriversAsync();

    }
}

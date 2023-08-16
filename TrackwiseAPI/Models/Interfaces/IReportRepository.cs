using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IReportRepository
    {
        Task<Job[]> GetLoadsCarriedAsync();
    }
}

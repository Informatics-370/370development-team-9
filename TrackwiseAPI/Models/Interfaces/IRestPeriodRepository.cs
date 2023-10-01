using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IRestPeriodRepository
    {
        void Update(RestPeriod rests);
        Task<bool> SaveChangesAsync();
        Task<RestPeriod> GetRestAsync();
    }
}

using TrackwiseAPI.Models.Entities;


namespace TrackwiseAPI.Models.Interfaces
{
    public interface IJobRuleRepository
    {
        Task<bool> SaveChangesAsync();
        Task<JobRule> GetRuleAsync();
    }
}

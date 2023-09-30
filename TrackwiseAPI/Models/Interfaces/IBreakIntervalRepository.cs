using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IBreakIntervalRepository
    {
        void Update(BreakInterval breaks);
        Task<bool> SaveChangesAsync();
        Task<BreakInterval> GetBreakAsync();
    }
}

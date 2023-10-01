using TrackwiseAPI.Models.Entities;

namespace TrackwiseAPI.Models.Interfaces
{
    public interface IHrsRepository
    {
        void Update(MaxHrs hours);
        Task<bool> SaveChangesAsync();
        Task<MaxHrs> GetHrsAsync();
    }
}

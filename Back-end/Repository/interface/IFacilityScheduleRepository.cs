using Clubly.Model;

namespace Clubly.Repository.Interfaces
{
    public interface IFacilityScheduleRepository
    {
        Task<List<FacilitySchedule>> GetAllAsync();
        Task<FacilitySchedule?> GetByIdAsync(int id);
        Task<FacilitySchedule> CreateAsync(FacilitySchedule schedule);
        Task UpdateAsync(FacilitySchedule schedule);
        Task DeleteAsync(int id);
    }
}

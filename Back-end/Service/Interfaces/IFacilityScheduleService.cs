using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IFacilityScheduleService
    {
        Task<List<FacilityScheduleDto>> GetAllAsync();
        Task<FacilityScheduleDto?> GetByIdAsync(int id);
        Task<FacilityScheduleDto> CreateAsync(CreateFacilityScheduleDto dto);
        Task<bool> UpdateAsync(int id, UpdateFacilityScheduleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

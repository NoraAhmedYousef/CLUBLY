using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IFacilityBookingService
    {
        Task<List<FacilityBookingDto>> GetAllAsync();
        Task<FacilityBookingDto?> GetByIdAsync(int id);
        Task<(FacilityBookingDto? result, string? error)> CreateAsync(CreateFacilityBookingDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateFacilityBookingStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

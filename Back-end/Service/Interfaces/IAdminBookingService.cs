using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IAdminBookingService
    {
        Task<List<ActivityBookingDto>> GetAllActivityBookingsAsync();
        Task<ActivityBookingDto?> GetActivityBookingByIdAsync(int id);
        Task<(bool success, string? error)> UpdateActivityBookingStatusAsync(int id, string status);

        Task<List<AdminFacilityBookingDto>> GetAllFacilityBookingsAsync();
        Task<AdminFacilityBookingDto?> GetFacilityBookingByIdAsync(int id);
        Task<(bool success, string? error)> UpdateFacilityBookingStatusAsync(int id, string status);
    }
}

using Clubly.DTO;

namespace Clubly.Repository.Interfaces
{
    public interface IAdminBookingRepository
    {
        Task<List<ActivityBookingDto>> GetAllActivityBookingsAsync();
        Task<ActivityBookingDto?> GetActivityBookingByIdAsync(int id);
        Task<bool> UpdateActivityBookingStatusAsync(int id, string status);

        Task<List<AdminFacilityBookingDto>> GetAllFacilityBookingsAsync();
        Task<AdminFacilityBookingDto?> GetFacilityBookingByIdAsync(int id);
        Task<bool> UpdateFacilityBookingStatusAsync(int id, string status);
    }
}

using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IActivityBookingService
    {
        Task<List<ActivityBookingDto>> GetAllAsync();
        Task<List<ActivityBookingDto>> GetByActivityAsync(int activityId);
        Task<List<ActivityBookingDto>> GetByMemberAsync(int memberId);
        Task<ActivityBookingDto?> GetByIdAsync(int id);
        Task<(ActivityBookingDto? result, string? error)> CreateAsync(CreateActivityBookingDto dto);
        Task<(bool success, string? error)> UpdateStatusAsync(int id, UpdateActivityBookingStatusDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

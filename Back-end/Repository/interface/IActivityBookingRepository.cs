using Clubly.Model;

namespace Clubly.Repository.Interfaces
{
   public interface IActivityBookingRepository
    {
        Task<List<ActivityBooking>> GetAllAsync();
        Task<List<ActivityBooking>> GetByActivityAsync(int activityId);
        Task<List<ActivityBooking>> GetByMemberAsync(int memberId);
        Task<ActivityBooking?> GetByIdAsync(int id);
        Task<bool> IsDuplicateAsync(int memberId, int activityGroupId);
        Task<ActivityBooking> CreateAsync(ActivityBooking booking);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }
}

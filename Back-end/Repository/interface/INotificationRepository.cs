using Clubly.DTO;
using Clubly.Model;

namespace Clubly.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(int id);
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification?> UpdateAsync(int id, Notification notification);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsReadAsync(int notifId, MarkReadDto dto);
        Task MarkReadAsync(int notifId, MarkReadDto dto);

        // ✅ متود جديد
        Task<IEnumerable<Notification>> GetByRoleAsync(string role);
        Task<DateTime> GetUserCreatedAtAsync(string role, int userId);


    }
}

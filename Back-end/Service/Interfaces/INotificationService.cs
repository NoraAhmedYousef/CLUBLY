using Clubly.DTO;
using Clubly.Model;

namespace Clubly.Service.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
        Task<NotificationResponseDto?> GetByIdAsync(int id);
        Task<NotificationResponseDto> CreateAsync(NotificationDto dto);
        Task<NotificationResponseDto?> UpdateAsync(int id, NotificationDto dto);
        Task<bool> DeleteAsync(int id);
        Task MarkReadAsync(int notifId, MarkReadDto dto);
        Task MarkAllReadAsync(MarkReadDto dto);
        Task<DateTime> GetUserCreatedAtAsync(string role, int userId);
        Task<IEnumerable<NotificationResponseDto>> GetForRoleAsync(string role, int userId, DateTime userCreatedAt);
    }
}
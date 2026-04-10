using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
        Task<NotificationResponseDto?> GetByIdAsync(int id);
        Task<NotificationResponseDto> CreateAsync(NotificationDto dto);
        Task<NotificationResponseDto?> UpdateAsync(int id, NotificationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

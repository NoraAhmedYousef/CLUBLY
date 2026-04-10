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
    }
}

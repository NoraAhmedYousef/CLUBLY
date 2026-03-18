using SignUp.Model;

namespace SignUp.Repository.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<List<Announcement>> GetAllAsync();
        Task<Announcement?> GetByIdAsync(int id);
        Task<Announcement> CreateAsync(Announcement announcement);
        Task<bool> UpdateAsync(Announcement announcement);
        Task<bool> DeleteAsync(int id);
    }
}

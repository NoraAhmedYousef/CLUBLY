using Clubly.Model;

namespace Clubly.Repository.Interfaces
{
   public interface IGuestRepository
    {
        Task<List<Guest>> GetAllAsync();
        Task<Guest?> GetByIdAsync(int id);
        Task AddAsync(Guest guest);
        Task UpdateAsync(Guest guest);
        Task DeleteAsync(Guest guest);
        Task<bool> ExistsAsync(int id);
        Task<Guest?> GetByEmailAsync(string email);
    }
}

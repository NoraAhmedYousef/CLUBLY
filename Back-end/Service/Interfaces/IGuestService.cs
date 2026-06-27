using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IGuestService
    {
        Task<List<GuestDto>> GetAllAsync();
        Task<GuestDto?> GetByIdAsync(int id);
        Task<GuestDto> CreateAsync(CreateGuestDto dto);
        Task<GuestDto?> UpdateAsync(int id, UpdateGuestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IAnnouncementService
    {
        Task<List<AnnouncementDto>> GetAllAsync();
        Task<AnnouncementDto?> GetByIdAsync(int id);
        Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto);
        Task<bool> UpdateAsync(int id, UpdateAnnouncementDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

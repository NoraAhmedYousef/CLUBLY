using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IActivityGroupService
    {
        Task<List<ActivityGroupDto>> GetAllAsync();
        Task<ActivityGroupDto?> GetByIdAsync(int id);
        Task<ActivityGroupDto> CreateAsync(CreateActivityGroupDto dto);
        Task<bool> UpdateAsync(int id, UpdateActivityGroupDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ActivityGroupDto>> GetByActivityIdAsync(int activityId);
    }
}

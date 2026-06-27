using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IActivityService
    {
        Task<List<Activity2Dto>> GetAllAsync();
        Task<Activity2Dto?> GetByIdDtoAsync(int id);
        Task<Activity> CreateAsync(CreateActivityDto dto);
        Task UpdateAsync(int id, UpdateActivityDto dto);
        Task DeleteAsync(int id);
        Task<List<ActivityDto>> GetAllForDash();
        Task<ActivityBookingDataDto?> GetActivityBookingDataAsync(int activityId);

    }
}

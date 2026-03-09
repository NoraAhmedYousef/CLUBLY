using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface ITrainerService
    {
        Task<List<TrainerDto>> GetAllAsync();
        Task<TrainerDto?> GetByIdAsync(int id);
        Task<TrainerDto> CreateAsync(CreateTrainerDto dto);
        Task<bool> UpdateAsync(int id, UpdateTrainerDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<TrainerDto>> GetByActivityIdAsync(int activityId);


    }
}

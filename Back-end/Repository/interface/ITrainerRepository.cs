using SignUp.Model;

namespace SignUp.Repository.Interfaces
{
    public interface ITrainerRepository
    {

        Task<List<Trainer>> GetAllAsync();
        Task<Trainer?> GetByIdAsync(int id);
        Task<Trainer> CreateAsync(Trainer trainer);
        Task UpdateAsync(Trainer trainer);
        Task DeleteAsync(Trainer trainer);
        Task<List<Trainer>> GetByActivityIdAsync(int activityId);


    }
}

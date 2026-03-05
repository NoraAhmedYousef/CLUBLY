using SignUp.Model;

namespace SignUp.Repository.Interfaces
{
    public interface IActivityGroupRepository
    {
        Task<List<ActivityGroup>> GetAllAsync();
        Task<ActivityGroup?> GetByIdAsync(int id);
        Task<ActivityGroup> CreateAsync(ActivityGroup group);
        Task<bool> UpdateAsync(ActivityGroup group);
        Task<bool> DeleteAsync(int id);
        Task<List<ActivityGroup>> GetByActivityIdAsync(int activityId);

    }
}

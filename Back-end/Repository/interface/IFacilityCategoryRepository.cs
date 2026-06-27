
using SignUp.Model;

namespace SignUp.Repository.Interfaces
{
   public interface IFacilityCategoryRepository
    {
        Task<List<FacilityCategory>> GetAllAsync();
        Task<FacilityCategory?> GetByIdAsync(int id);
        Task<FacilityCategory> CreateAsync(FacilityCategory entity);
        Task<bool> UpdateAsync(int id, FacilityCategory entity);
        Task<bool> DeleteAsync(int id);

    }
}

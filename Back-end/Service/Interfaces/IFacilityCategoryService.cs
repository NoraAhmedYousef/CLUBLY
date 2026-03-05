using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IFacilityCategoryService 
    {

        Task<List<FacilityCategory>> GetAllAsync();
        Task<List<FacilityCategoryDto>> GetAllForDash();
        Task<FacilityCategory?> GetByIdAsync(int id);
        Task<FacilityCategory> CreateAsync(CreateFacilityCategoryDto dto);
        Task<bool> UpdateAsync(int id, UpdateFacilityCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

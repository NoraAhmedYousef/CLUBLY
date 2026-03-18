using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IFacilityService
    {

        Task<List<Facility2Dto>> GetAllAsync();
        Task<Facility2Dto?> GetByIdDtoAsync(int id);
        Task<Facility> CreateAsync(CreateFacilityDto dto);
        Task UpdateAsync(int id, UpdateFacilityDto dto);
        Task DeleteAsync(int id);
        Task<List<FacilityDto>> GetAllForDash();

    }
}

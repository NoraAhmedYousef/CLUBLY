using SignUp.DTO;

namespace SignUp.Service.Interfaces
{
    public interface IAdminService
    {

        Task<List<AdminDto>> GetAllAdminsAsync();
        Task<AdminDto> GetAdminByIdAsync(int id);
        Task<AdminDto> CreateAdminAsync(CreateAdminDto dto);
        Task<AdminDto> UpdateAdminAsync(int id, UpdateAdminDto dto);
        Task<bool> DeleteAdminAsync(int id);
    }
}

using SignUp.Model;

namespace SignUp.Repository.Interface
{
    public interface IAdminRepository
    {
        Task<List<Admin>> GetAllAsync();
        Task<Admin> GetByIdAsync(int id);
        Task<Admin> CreateAsync(Admin admin);
        Task<Admin> UpdateAsync(Admin admin);
        Task<bool> DeleteAsync(int id);
    }
}

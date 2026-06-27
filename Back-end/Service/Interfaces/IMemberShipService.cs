
using SignUp.DTO;
using SignUp.Model;

namespace SignUp.Service.Interfaces
{
    public interface IMemberShipService
    {
        Task<List<MemberShip>> GetAllAsync();
        Task<MemberShip?> GetByIdAsync(int id);
        Task<MemberShip> CreateAsync(CreateMemberShipDto dto);
        Task<bool> UpdateAsync(int id, UpdateMemberShipDto dto);
        Task<bool> DeleteAsync(int id);
    }
}

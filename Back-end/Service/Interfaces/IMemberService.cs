using SignUp.DTO;
using SignUp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignUp.Service.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member2Dto>> GetAllAsync();
        Task<Member2Dto?> GetByIdAsync(int id);
        Task<Member2Dto> CreateAsync(CreateMemberDto dto);
        Task<Member2Dto?> UpdateAsync(int id, UpdateMemberDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
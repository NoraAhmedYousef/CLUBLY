using SignUp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignUp.Repository.Interfaces
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(Member member);
        Task<bool> ExistsAsync(int id);
    }
}

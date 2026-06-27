using Clubly.Model;

namespace Clubly.Repository.Interfaces
{
public interface IMembershipRenewalRequestRepository
    {
        Task<List<MembershipRenewalRequest>> GetAllAsync();
        Task<MembershipRenewalRequest?> GetByIdAsync(int id);
        Task<List<MembershipRenewalRequest>> GetByMemberAsync(int memberId);
        Task<MembershipRenewalRequest> CreateAsync(MembershipRenewalRequest request);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}

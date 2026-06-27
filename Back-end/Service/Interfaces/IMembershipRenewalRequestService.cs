using Clubly.DTO;

namespace Clubly.Service.Interfaces
{
    public interface IMembershipRenewalRequestService
    {
        Task<List<MembershipRenewalRequestDto>> GetAllAsync();
        Task<MembershipRenewalRequestDto?> GetByIdAsync(int id);
        Task<List<MembershipRenewalRequestDto>> GetByMemberAsync(int memberId);
        Task<MembershipRenewalRequestDto> CreateAsync(CreateMembershipRenewalRequestDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateMembershipRenewalStatusDto dto);
        Task DeleteAsync(int id);
    }
}

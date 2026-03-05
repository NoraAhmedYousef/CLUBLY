using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;
using System.Diagnostics;

namespace SignUp.Service.Class
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IMemberShipRepository _repo;
        private readonly IMemberRepository _memberrepo;


        public MemberShipService(IMemberShipRepository repo, IMemberRepository memberrepo)
        {
            _repo = repo;
            _memberrepo = memberrepo;
        }

        public async Task<List<MemberShip>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<MemberShip?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<MemberShip> CreateAsync(CreateMemberShipDto dto)
        {
            var memberShip = new MemberShip
            {
                Name = dto.Name,
                Description = dto.Description,
                Duration = dto.Duration,
                Price = dto.Price,
                Status = dto.Status
            };

            return await _repo.CreateAsync(memberShip);
        }

        public async Task<bool> UpdateAsync(int id, UpdateMemberShipDto dto)
        {
            var memberShip = await _repo.GetByIdAsync(id);
            if (memberShip == null) return false;

            // Apply updates only if provided
            if (dto.Name != null) memberShip.Name = dto.Name;
            if (dto.Description != null) memberShip.Description = dto.Description;
            if (dto.Duration.HasValue)
                memberShip.Duration = dto.Duration.Value;
            if (dto.Price.HasValue) memberShip.Price = dto.Price.Value;
            if (dto.Status != null) memberShip.Status = dto.Status;

            await _repo.UpdateAsync(memberShip);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
}

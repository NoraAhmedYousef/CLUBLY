using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class FacilityCategoryService : IFacilityCategoryService
    {
        private readonly IFacilityCategoryRepository _repo;
        public FacilityCategoryService(IFacilityCategoryRepository repo) => _repo = repo;

        public async Task<List<FacilityCategory>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<FacilityCategory?> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<FacilityCategory> CreateAsync(CreateFacilityCategoryDto dto)
        {
            var entity = new FacilityCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status
            };
            return await _repo.CreateAsync(entity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateFacilityCategoryDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Name)) entity.Name = dto.Name;
            if (dto.Description != null) entity.Description = dto.Description;
            if (!string.IsNullOrWhiteSpace(dto.Status)) entity.Status = dto.Status;

            return await _repo.UpdateAsync(id, entity);
        }

        public async Task<bool> DeleteAsync(int id)
        
            => await _repo.DeleteAsync(id);
        public async Task<List<FacilityCategoryDto>> GetAllForDash()
        {
            var Category = await _repo.GetAllAsync();
            return Category.Select(f => new FacilityCategoryDto { Id = f.Id, Name = f.Name }).ToList();
        }
    }
  
    } 


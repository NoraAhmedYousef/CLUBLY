
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _repo;
        private readonly IWebHostEnvironment _env;

        public FacilityService(IFacilityRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "Uploads", "Facilities");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/Uploads/Facilities/{fileName}";
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<List<Facility2Dto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            return data.Select(f => new Facility2Dto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Capacity = f.Capacity,
                FacilityCategoryId = f.FacilityCategoryId ?? 0,
                FacilityCategoryName = f.FacilityCategory?.Name,
                Status = f.Status,
                ImageUrl = f.ImageUrl

            }).ToList();
        }

        public async Task<Facility2Dto?> GetByIdDtoAsync(int id)
        {
            var f = await _repo.GetByIdAsync(id);
            if (f == null) return null;

            return new Facility2Dto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Capacity = f.Capacity,
                FacilityCategoryId = f.FacilityCategoryId ?? 0,
                FacilityCategoryName = f.FacilityCategory?.Name,
                Status = f.Status,
                ImageUrl = f.ImageUrl
            };
        }

        public async Task<Facility> CreateAsync(CreateFacilityDto dto)
        {
            var imageUrl = await SaveImageAsync(dto.Image);

            var facility = new Facility
            {
                Name = dto.Name,
                Description = dto.Description,
                Capacity = dto.Capacity,
                FacilityCategoryId = dto.FacilityCategoryId,
                Status = dto.Status,
                ImageUrl = imageUrl
            };

            return await _repo.CreateAsync(facility);
        }

        public async Task UpdateAsync(int id, UpdateFacilityDto dto)
        {
            var facility = await _repo.GetByIdAsync(id);
            if (facility == null) return;

            if (dto.Name != null) facility.Name = dto.Name;
            if (dto.Description != null) facility.Description = dto.Description;
            if (dto.Capacity.HasValue) facility.Capacity = dto.Capacity.Value;
            if (dto.FacilityCategoryId.HasValue) facility.FacilityCategoryId = dto.FacilityCategoryId.Value;
            if (dto.Status != null) facility.Status = dto.Status;

            if (dto.Image != null)
            {
                DeleteOldImage(facility.ImageUrl);
                facility.ImageUrl = await SaveImageAsync(dto.Image);
            }

            await _repo.UpdateAsync(facility);
        }

        public async Task DeleteAsync(int id)
        {
            var facility = await _repo.GetByIdAsync(id);

            if (facility?.ImageUrl != null)
                DeleteOldImage(facility.ImageUrl);

            await _repo.DeleteAsync(id);
        }

        public async Task<List<FacilityDto>> GetAllForDash()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(x => new FacilityDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
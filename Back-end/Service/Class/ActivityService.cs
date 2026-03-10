using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;


        public ActivityService(IActivityRepository repo, IWebHostEnvironment env, AppDbContext context)
        {
            _repo = repo;
            _env = env;
            _context = context;
        }

        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "Uploads", "Activities");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/Uploads/Activities/{fileName}";
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<List<Activity2Dto>> GetAllAsync()
        {
            var activities = await _repo.GetAllAsync();

            return activities.Select(a => new Activity2Dto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                FacilityId = a.FacilityId,
                FacilityName = a.Facility?.Name ?? "Not assigned",
                Status = a.Status,
                ImageUrl = a.ImageUrl
            }).ToList();
        }

        public async Task<Activity2Dto?> GetByIdDtoAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new Activity2Dto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                FacilityId = a.FacilityId,
                FacilityName = a.Facility?.Name ?? "Not assigned",
                Status = a.Status,
                ImageUrl = a.ImageUrl
            };
        }

        public async Task<Activity> CreateAsync(CreateActivityDto dto)
        {
            var imageUrl = await SaveImageAsync(dto.Image);

            var activity = new Activity
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                FacilityId = dto.FacilityId,
                Status = dto.Status,
                ImageUrl = imageUrl
            };

            return await _repo.CreateAsync(activity);
        }

        public async Task UpdateAsync(int id, UpdateActivityDto dto)
        {
            var activity = await _repo.GetByIdAsync(id);
            if (activity == null) return;

            if (dto.Name != null) activity.Name = dto.Name;
            if (dto.Description != null) activity.Description = dto.Description;
            if (dto.Price.HasValue) activity.Price = dto.Price.Value;
            if (dto.FacilityId.HasValue) activity.FacilityId = dto.FacilityId.Value;
            if (dto.Status != null) activity.Status = dto.Status;

            if (dto.Image != null)
            {
                DeleteOldImage(activity.ImageUrl);
                activity.ImageUrl = await SaveImageAsync(dto.Image);
            }

            await _repo.UpdateAsync(activity);
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _repo.GetByIdAsync(id);
            if (activity?.ImageUrl != null)
                DeleteOldImage(activity.ImageUrl);

            await _repo.DeleteAsync(id);
        }

        public async Task<List<ActivityDto>> GetAllForDash()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(x => new ActivityDto { Id = x.Id, Name = x.Name }).ToList();
        }

        public async Task<ActivityBookingDataDto?> GetActivityBookingDataAsync(int activityId)
        {
            var activity = await _context.Activities
                .Include(a => a.Trainers)
                .Include(a => a.ActivityGroups)
                .FirstOrDefaultAsync(a => a.Id == activityId && a.Status == "Active");

            if (activity == null)
                return null;

            return new ActivityBookingDataDto
            {
                ActivityId = activity.Id,
                ActivityName = activity.Name,
                Price = activity.Price,
                ImageUrl = activity.ImageUrl,
                Trainers = activity.Trainers
                    .Where(t => t.IsActive)
                    .Select(t => new TrainerOptionDto
                    {
                        Id = t.Id,
                        FullName = t.FullName,
                        ImageUrl = t.ImageUrl,
                        YearsOfExperience = t.YearsOfExperience,
                        Phone = t.Phone,
                        Email = t.Email
                    })
                    .OrderBy(t => t.FullName)
                    .ToList(),
                Groups = activity.ActivityGroups
    .Select(g => new GroupOptionDto
    {
        Id = g.Id,
        Name = g.Name,
        Code = g.Code,
        Capacity = g.Capacity,
        Day = g.Day,
        Duration = g.Duration
    })
    .OrderBy(g => g.Day)
    .ToList()
            };
        }

    }
}




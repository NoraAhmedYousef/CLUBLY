
using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interfaces;
using SignUp.Service.Interfaces;

namespace SignUp.Service.Class
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public TrainerService(ITrainerRepository repo, IWebHostEnvironment env, AppDbContext context)
        {
            _repo = repo;
            _env = env;
            _context = context;
        }

        // ------------------- PASSWORD METHODS -------------------

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(Convert.FromBase64String(storedSalt));
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computedHash) == storedHash;
        }

        // ------------------- IMAGE METHODS -------------------

        private async Task<string?> SaveImageAsync(IFormFile? image, string folderName)
        {
            if (image == null || image.Length == 0) return null;

            var folder = Path.Combine(_env.WebRootPath, "Uploads", folderName);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            return $"/Uploads/{folderName}/{fileName}";
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;
            var fullPath = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        // ------------------- CRUD METHODS -------------------

        public async Task<List<TrainerDto>> GetAllAsync()
        {
            var trainers = await _context.Trainers
                .Include(t => t.Activities)
                .ToListAsync();

            return trainers.Select(t => new TrainerDto
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                YearsOfExperience = t.YearsOfExperience,
                NationalId = t.NationalId,
                DateOfBirth = t.DateOfBirth,
                Gender = t.Gender,
                Activities = t.Activities.Any()
                    ? string.Join(", ", t.Activities.Select(a => a.Name))
                    : "N/A",
                ActivityIds = t.Activities.Select(a => a.Id).ToList(), // ⭐ إضافة هذا السطر
                IsActive = t.IsActive,
                ImageUrl = t.ImageUrl
            }).ToList();
        }
        public async Task<TrainerDto?> GetByIdAsync(int id)
        {
            var t = await _context.Trainers
                .Include(tr => tr.Activities)
                .FirstOrDefaultAsync(tr => tr.Id == id);

            if (t == null) return null;

            return new TrainerDto
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Phone = t.Phone,
                YearsOfExperience = t.YearsOfExperience,
                NationalId = t.NationalId,
                DateOfBirth = t.DateOfBirth,
                Gender = t.Gender,

                Activities = t.Activities.Any() ? string.Join(", ", t.Activities.Select(a => a.Name)) : "N/A",
                ActivityIds = t.Activities.Select(a => a.Id).ToList(),

                IsActive = t.IsActive,
                ImageUrl = t.ImageUrl
            };
        }
        public async Task<TrainerDto> CreateAsync(CreateTrainerDto dto)
        {
            var imageUrl = await SaveImageAsync(dto.Image, "Trainers");

            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var trainer = new Trainer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                YearsOfExperience = dto.YearsOfExperience,
                NationalId = dto.NationalId,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                IsActive = dto.IsActive,
                ImageUrl = imageUrl,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            // ⭐ ربط الأنشطة
            if (dto.ActivityIds != null && dto.ActivityIds.Any())
            {
                trainer.Activities = await _context.Activities
                    .Where(a => dto.ActivityIds.Contains(a.Id))
                    .ToListAsync();
            }

            await _repo.CreateAsync(trainer);

            return await GetByIdAsync(trainer.Id)
                ?? throw new Exception("Trainer not found after creation");
        }
        public async Task<bool> UpdateAsync(int id, UpdateTrainerDto dto)
        {
            var t = await _context.Trainers
                .Include(tr => tr.Activities)
                .FirstOrDefaultAsync(tr => tr.Id == id);

            if (t == null) return false;

            t.FullName = dto.FullName ?? t.FullName;
            t.Email = dto.Email ?? t.Email;
            t.Phone = dto.Phone ?? t.Phone;
            t.YearsOfExperience = dto.YearsOfExperience ?? t.YearsOfExperience;
            t.NationalId = dto.NationalId ?? t.NationalId;
            t.DateOfBirth = dto.DateOfBirth ?? t.DateOfBirth;
            t.Gender = dto.Gender ?? t.Gender;
            t.IsActive = dto.IsActive ?? t.IsActive;

            // ⭐ تحديث الأنشطة لو اتبعتّ IDs
            if (dto.ActivityIds != null)
            {
                t.Activities = await _context.Activities
                    .Where(a => dto.ActivityIds.Contains(a.Id))
                    .ToListAsync();
            }

            // ⭐ Update Image
            if (dto.Image != null)
            {
                DeleteOldImage(t.ImageUrl);
                t.ImageUrl = await SaveImageAsync(dto.Image, "Trainers");
            }

            // ⭐ Update password
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                CreatePasswordHash(dto.NewPassword, out string hash, out string salt);
                t.PasswordHash = hash;
                t.PasswordSalt = salt;
            }

            await _repo.UpdateAsync(t);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return false;

            DeleteOldImage(t.ImageUrl);
            await _repo.DeleteAsync(t);
            return true;
        }
    }
}


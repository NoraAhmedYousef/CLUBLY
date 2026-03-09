using SignUp.DTO;
using SignUp.Model;
using SignUp.Repository.Interface;
using SignUp.Service.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SignUp.Service.Class
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _repo;
        private readonly IWebHostEnvironment _env;

        public AdminService(IAdminRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
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

        public async Task<List<AdminDto>> GetAllAdminsAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(a => new AdminDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                Phone = a.Phone,
                NationalId = a.NationalId,
                Gender = a.Gender,
                DateOfBirth = a.DateOfBirth,
                ImageUrl = a.ImageUrl
            }).ToList();
        }

        public async Task<AdminDto?> GetAdminByIdAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return null;

            return new AdminDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email,
                Phone = a.Phone,
                NationalId = a.NationalId,
                Gender = a.Gender,
                DateOfBirth = a.DateOfBirth,
                ImageUrl = a.ImageUrl
            };
        }

        public async Task<AdminDto> CreateAdminAsync(CreateAdminDto dto)
        {
            var imageUrl = await SaveImageAsync(dto.Image, "Admins");

            // -------- create password hash ----------
            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var admin = new Admin
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                NationalId = dto.NationalId,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                ImageUrl = imageUrl,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _repo.CreateAsync(admin);

            return await GetAdminByIdAsync(admin.Id)
                ?? throw new Exception("Admin not found after creation");
        }

        public async Task<AdminDto> UpdateAdminAsync(int id, UpdateAdminDto dto)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) throw new Exception("Admin not found");

            a.FullName = dto.FullName ?? a.FullName;
            a.Email = dto.Email ?? a.Email;
            a.Phone = dto.Phone ?? a.Phone;
            a.NationalId = dto.NationalId ?? a.NationalId;
            a.Gender = dto.Gender ?? a.Gender;
            a.DateOfBirth = dto.DateOfBirth ?? a.DateOfBirth;

            // -------- update image ----------
            if (dto.Image != null)
            {
                DeleteOldImage(a.ImageUrl);
                a.ImageUrl = await SaveImageAsync(dto.Image, "Admins");
            }

            // -------- update password ----------
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                CreatePasswordHash(dto.NewPassword, out string hash, out string salt);
                a.PasswordHash = hash;
                a.PasswordSalt = salt;
            }

            await _repo.UpdateAsync(a);

            return await GetAdminByIdAsync(a.Id)
                ?? throw new Exception("Admin not found after update");
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            var a = await _repo.GetByIdAsync(id);
            if (a == null) return false;

            DeleteOldImage(a.ImageUrl);
            return await _repo.DeleteAsync(id);
        }
    }
}
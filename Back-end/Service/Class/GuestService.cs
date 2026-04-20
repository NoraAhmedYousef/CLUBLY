using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;

namespace Clubly.Service.Class
{
    public class GuestService: IGuestService
    {
        private readonly IGuestRepository _repo;

        public GuestService(IGuestRepository repo)
        {
            _repo = repo;
        }

        // GET ALL
        public async Task<List<GuestDto>> GetAllAsync()
        {
            var guests = await _repo.GetAllAsync();
            return guests.Select(ToDto).ToList();
        }

        // GET BY ID
        public async Task<GuestDto?> GetByIdAsync(int id)
        {
            var g = await _repo.GetByIdAsync(id);
            return g == null ? null : ToDto(g);
        }

        // CREATE
        public async Task<GuestDto> CreateAsync(CreateGuestDto dto)
        {
            var guest = new Guest
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                NationalId = dto.NationalId,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTime.UtcNow
            };

            CreatePasswordHash(dto.Password, out string hash, out string salt);
            guest.PasswordHash = hash;
            guest.PasswordSalt = salt;

            await _repo.AddAsync(guest);
            return ToDto(guest);
        }

        // UPDATE
        public async Task<GuestDto?> UpdateAsync(int id, UpdateGuestDto dto)
        {
            var g = await _repo.GetByIdAsync(id);
            if (g == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.FirstName)) g.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) g.LastName = dto.LastName;
            if (!string.IsNullOrWhiteSpace(dto.Email)) g.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) g.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.NationalId)) g.NationalId = dto.NationalId;
            if (!string.IsNullOrWhiteSpace(dto.Gender)) g.Gender = dto.Gender;
            if (dto.DateOfBirth.HasValue) g.DateOfBirth = dto.DateOfBirth.Value;

            // Image
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "guests");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                g.ImageUrl = $"/uploads/guests/{fileName}";
            }

            // Password
            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                if (!string.IsNullOrWhiteSpace(dto.CurrentPassword))
                {
                    if (!VerifyPassword(dto.CurrentPassword, g.PasswordHash, g.PasswordSalt))
                        throw new Exception("Current password is incorrect");
                }
                CreatePasswordHash(dto.NewPassword, out string hash, out string salt);
                g.PasswordHash = hash;
                g.PasswordSalt = salt;
            }

            await _repo.UpdateAsync(g);
            return ToDto(g);
        }
        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var g = await _repo.GetByIdAsync(id);
            if (g == null) return false;

            await _repo.DeleteAsync(g);
            return true;
        }

        // MAPPER
        private static GuestDto ToDto(Guest g) => new GuestDto
        {
            Id = g.Id,
            FirstName = g.FirstName,
            LastName = g.LastName,
            FullName = g.FullName,
            Email = g.Email,
            Phone = g.Phone,
            NationalId = g.NationalId,
            Gender = g.Gender,
            DateOfBirth = g.DateOfBirth,
            ImageUrl = g.ImageUrl,   // ← أضفها

            CreatedAt = g.CreatedAt
        };

        // PASSWORD HELPERS — نفس طريقة الـ Member
        private void CreatePasswordHash(string password, out string hash, out string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            hash = Convert.ToBase64String(
                   hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var key = Convert.FromBase64String(storedSalt);
            using var hmac = new System.Security.Cryptography.HMACSHA512(key);
            var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(computed) == storedHash;
        }
    }
}

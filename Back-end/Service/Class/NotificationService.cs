using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;

namespace Clubly.Service.Class
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IWebHostEnvironment _env;
        public NotificationService(INotificationRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        private static NotificationResponseDto ToDto(Notification n) => new()
        {
            Id = n.Id,
            Title = n.Title,
            Description = n.Description,
            SentAt = n.SentAt,
            Status = n.Status,
            ToMembers = n.ToMembers,
            ToTrainers = n.ToTrainers,
            ToAdmins = n.ToAdmins,
            ToGuests = n.ToGuests,
            Type = n.Type,
            ImageUrl = n.ImageUrl
        };

        public async Task<IEnumerable<NotificationResponseDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToDto);

        public async Task<NotificationResponseDto?> GetByIdAsync(int id)
        {
            var n = await _repo.GetByIdAsync(id);
            return n == null ? null : ToDto(n);
        }

        public async Task<NotificationResponseDto> CreateAsync(NotificationDto dto)
        {
            // ✅ ارفع الصورة لو موجودة
            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "notifications");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                imageUrl = $"/uploads/notifications/{fileName}";
            }

            var n = new Notification
            {
                Title = dto.Title,
                Description = dto.Description,
                ToMembers = dto.ToMembers,
                ToTrainers = dto.ToTrainers,
                ToAdmins = dto.ToAdmins,
                ToGuests = dto.ToGuests,
                Type = dto.Type ?? "Info",
                ImageUrl = imageUrl,
                SentAt = DateTime.UtcNow,
                Status = "Sent"
            };

            return ToDto(await _repo.CreateAsync(n));
        }

        public async Task<NotificationResponseDto?> UpdateAsync(int id, NotificationDto dto)
        {
            // ✅ لو في صورة جديدة ارفعها
            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "notifications");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
                imageUrl = $"/uploads/notifications/{fileName}";
            }

            var n = new Notification
            {
                Title = dto.Title,
                Description = dto.Description,
                ToMembers = dto.ToMembers,
                ToTrainers = dto.ToTrainers,
                ToAdmins = dto.ToAdmins,
                ToGuests = dto.ToGuests,
                Type = dto.Type ?? "Info",
                ImageUrl = imageUrl
            };

            var result = await _repo.UpdateAsync(id, n);
            return result == null ? null : ToDto(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // ✅ احذف الصورة من الـ server لو موجودة
            var existing = await _repo.GetByIdAsync(id);
            if (existing?.ImageUrl != null)
            {
                var filePath = Path.Combine(_env.WebRootPath, existing.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath)) File.Delete(filePath);
            }
            return await _repo.DeleteAsync(id);
        }
    }
}
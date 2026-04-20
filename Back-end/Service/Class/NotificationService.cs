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
        public async Task MarkReadAsync(int notifId, MarkReadDto dto)
        {
            var isRead = await _repo.IsReadAsync(notifId, dto);
            if (!isRead)
                await _repo.MarkReadAsync(notifId, dto);
        }

        public async Task MarkAllReadAsync(MarkReadDto dto)
        {
            // استنتج الـ role من الـ dto
            string role = dto.AdminId.HasValue ? "Admin"
                        : dto.TrainerId.HasValue ? "Trainer"
                        : dto.MemberId.HasValue ? "Member"
                        : "Guest";

            var notifications = await _repo.GetByRoleAsync(role);

            foreach (var n in notifications)
            {
                var isRead = await _repo.IsReadAsync(n.Id, dto);
                if (!isRead)
                    await _repo.MarkReadAsync(n.Id, dto);
            }
        }
        public async Task<DateTime> GetUserCreatedAtAsync(string role, int userId)
        {
            return await _repo.GetUserCreatedAtAsync(role, userId);
        }

        public async Task<IEnumerable<NotificationResponseDto>> GetForRoleAsync(string role, int userId, DateTime userCreatedAt)
        {
            var notifications = await _repo.GetByRoleAsync(role);

            // ✅ فلتر بتاريخ انضمام الـ user
            notifications = notifications.Where(n => n.SentAt >= userCreatedAt);

            var dto = role.ToLower() switch
            {
                "admin" => new MarkReadDto { AdminId = userId },
                "trainer" => new MarkReadDto { TrainerId = userId },
                "member" => new MarkReadDto { MemberId = userId },
                _ => new MarkReadDto { GuestId = userId }
            };

            var result = new List<NotificationResponseDto>();
            foreach (var n in notifications)
            {
                var notifDto = ToDto(n);
                notifDto.IsRead = await _repo.IsReadAsync(n.Id, dto);
                result.Add(notifDto);
            }
            return result;
        }
     
    }
}
using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Notification>> GetAllAsync() =>
            await _context.Notifications
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();

        public async Task<Notification?> GetByIdAsync(int id) =>
            await _context.Notifications.FindAsync(id);

        public async Task<Notification> CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> UpdateAsync(int id, Notification updated)
        {
            var existing = await _context.Notifications.FindAsync(id);
            if (existing == null) return null;

            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.ToMembers = updated.ToMembers;
            existing.ToTrainers = updated.ToTrainers;
            existing.ToAdmins = updated.ToAdmins;
            existing.ToGuests = updated.ToGuests;
            existing.Status = updated.Status;

            if (updated.Type != null) existing.Type = updated.Type;
            if (updated.ImageUrl != null) existing.ImageUrl = updated.ImageUrl;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var n = await _context.Notifications.FindAsync(id);
            if (n == null) return false;
            _context.Notifications.Remove(n);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ إصلاح: أقواس صحيحة
        public async Task<bool> IsReadAsync(int notifId, MarkReadDto dto)
        {
            return await _context.NotificationReads.AnyAsync(r =>
                r.NotificationId == notifId &&
                (
                    (dto.GuestId != null && r.GuestId == dto.GuestId) ||
                    (dto.MemberId != null && r.MemberId == dto.MemberId) ||
                    (dto.TrainerId != null && r.TrainerId == dto.TrainerId) ||
                    (dto.AdminId != null && r.AdminId == dto.AdminId)
                ));
        }

        public async Task MarkReadAsync(int notifId, MarkReadDto dto)
        {
            var alreadyRead = await IsReadAsync(notifId, dto);
            if (alreadyRead) return;

            _context.NotificationReads.Add(new NotificationRead
            {
                NotificationId = notifId,
                GuestId = dto.GuestId,
                MemberId = dto.MemberId,
                TrainerId = dto.TrainerId,
                AdminId = dto.AdminId,
                ReadAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        // ✅ جديد: مطلوب لـ MarkAllReadAsync و GetForRoleAsync في الـ Service
        public async Task<IEnumerable<Notification>> GetByRoleAsync(string role)
        {
            var query = _context.Notifications.AsQueryable();

            query = role.ToLower() switch
            {
                "guest" => query.Where(n => n.ToGuests),
                "member" => query.Where(n => n.ToMembers),
                "trainer" => query.Where(n => n.ToTrainers),
                "admin" => query.Where(n => n.ToAdmins),
                _ => query.Where(n => false)
            };

            return await query
                .OrderByDescending(n => n.SentAt)
                .ToListAsync();
        }
        public async Task<DateTime> GetUserCreatedAtAsync(string role, int userId)
        {
            return role.ToLower() switch
            {
                "admin" => await _context.Admins
                                .Where(u => u.Id == userId)
                                .Select(u => u.CreatedAt)
                                .FirstOrDefaultAsync(),

                "trainer" => await _context.Trainers
                                .Where(u => u.Id == userId)
                                .Select(u => u.CreatedAt)
                                .FirstOrDefaultAsync(),

                "member" => await _context.Members
                                .Where(u => u.Id == userId)
                                .Select(u => u.CreatedAt)
                                .FirstOrDefaultAsync(),

                _ => await _context.Guests
                                .Where(u => u.Id == userId)
                                .Select(u => u.CreatedAt)
                                .FirstOrDefaultAsync(),
            };
        }
    }
}
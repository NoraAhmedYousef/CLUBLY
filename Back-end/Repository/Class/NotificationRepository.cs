using Clubly.Model;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Notification>> GetAllAsync() =>
            await _context.Notifications.OrderByDescending(n => n.SentAt).ToListAsync();

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
            if (updated.ImageUrl != null)
                existing.ImageUrl = updated.ImageUrl;
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
    }
}

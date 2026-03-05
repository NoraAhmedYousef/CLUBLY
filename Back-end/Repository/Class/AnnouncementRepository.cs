using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using SignUp.Repository.Interfaces;

namespace SignUp.Repository.Class
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly AppDbContext _db;

        public AnnouncementRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Announcement>> GetAllAsync()
        {
            return await _db.Announcements.OrderByDescending(a => a.PublishDate).ToListAsync();
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _db.Announcements.FindAsync(id);
        }

        public async Task<Announcement> CreateAsync(Announcement announcement)
        {
            await _db.Announcements.AddAsync(announcement);
            await _db.SaveChangesAsync();
            return announcement;
        }

        public async Task<bool> UpdateAsync(Announcement announcement)
        {
            _db.Announcements.Update(announcement);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var a = await _db.Announcements.FindAsync(id);
            if (a == null) return false;

            _db.Announcements.Remove(a);
            return await _db.SaveChangesAsync() > 0;
        }
    }
    }


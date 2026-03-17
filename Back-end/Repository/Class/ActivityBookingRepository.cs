using Clubly.Model;
using Clubly.Repository.Interface;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class ActivityBookingRepository :IActivityBookingRepository
    {
        private readonly AppDbContext _db;
    public ActivityBookingRepository(AppDbContext db) => _db = db;

    public async Task<List<ActivityBooking>> GetAllAsync() =>
        await _db.ActivityBookings
                 .Include(b => b.Activity)
                 .Include(b => b.ActivityGroup)
                 .Include(b => b.Member)
                 .OrderByDescending(b => b.CreatedAt)
                 .ToListAsync();

    public async Task<List<ActivityBooking>> GetByActivityAsync(int activityId) =>
        await _db.ActivityBookings
                 .Include(b => b.Activity)
                 .Include(b => b.ActivityGroup)
                 .Include(b => b.Member)
                 .Where(b => b.ActivityId == activityId)
                 .OrderByDescending(b => b.CreatedAt)
                 .ToListAsync();

    public async Task<List<ActivityBooking>> GetByMemberAsync(int memberId) =>
        await _db.ActivityBookings
                 .Include(b => b.Activity)
                 .Include(b => b.ActivityGroup)
                 .Where(b => b.MemberId == memberId)
                 .OrderByDescending(b => b.CreatedAt)
                 .ToListAsync();

    public async Task<ActivityBooking?> GetByIdAsync(int id) =>
        await _db.ActivityBookings
                 .Include(b => b.Activity)
                 .Include(b => b.ActivityGroup)
                 .Include(b => b.Member)
                 .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<bool> IsDuplicateAsync(int memberId, int activityGroupId) =>
        await _db.ActivityBookings.AnyAsync(b =>
            b.MemberId == memberId &&
            b.ActivityGroupId == activityGroupId &&
            b.Status != "Cancelled");

    public async Task<ActivityBooking> CreateAsync(ActivityBooking booking)
    {
        _db.ActivityBookings.Add(booking);
        await _db.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var booking = await _db.ActivityBookings.FindAsync(id);
        if (booking is null) return false;
        booking.Status = status;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _db.ActivityBookings.FindAsync(id);
        if (booking is null) return false;
        _db.ActivityBookings.Remove(booking);
        await _db.SaveChangesAsync();
        return true;
    }
}
}
using Microsoft.EntityFrameworkCore;
using Clubly.Repository.Interface;
using SignUp.Data;
using SignUp.Model;


namespace Clubly.Repository.Class
{
    public class FacilityBookingRepository : IFacilityBookingRepository

    {
        private readonly AppDbContext _db;
        public FacilityBookingRepository(AppDbContext db) => _db = db;

        public async Task<List<FacilityBooking>> GetAllAsync() =>
            await _db.FacilityBookings
            .Include(b => b.Member)
                     .Include(b => b.Facility)
                     .Include(b => b.Schedule)
                     .OrderByDescending(b => b.BookingDate)
                     .ThenBy(b => b.StartTime)
                     .ToListAsync();

        public async Task<FacilityBooking?> GetByIdAsync(int id) =>
            await _db.FacilityBookings
                     .Include(b => b.Facility)
                     .Include(b => b.Schedule)
                     .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<List<FacilityBooking>> GetByFacilityAsync(int facilityId) =>
            await _db.FacilityBookings
                     .Where(b => b.FacilityId == facilityId && b.Status != "Cancelled")
                     .OrderBy(b => b.BookingDate).ThenBy(b => b.StartTime)
                     .ToListAsync();

        // تحقق إن مفيش حجز تاني في نفس الوقت لنفس الـ facility
        public async Task<bool> HasConflictAsync(int facilityId, DateOnly date, TimeOnly start, TimeOnly end, int? excludeId = null) =>
            await _db.FacilityBookings.AnyAsync(b =>
                b.FacilityId == facilityId &&
                b.BookingDate == date &&
                b.Status != "Cancelled" &&
                (excludeId == null || b.Id != excludeId) &&
                b.StartTime < end && b.EndTime > start);

        public async Task<FacilityBooking> CreateAsync(FacilityBooking booking)
        {
            _db.FacilityBookings.Add(booking);
            await _db.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var b = await _db.FacilityBookings.FindAsync(id);
            if (b is null) return;
            b.Status = status;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var b = await _db.FacilityBookings.FindAsync(id);
            if (b is null) return;
            _db.FacilityBookings.Remove(b);
            await _db.SaveChangesAsync();
        }
    }
}
    
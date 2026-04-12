using Clubly.DTO;
using Clubly.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Repository.Class
{
    public class AdminBookingRepository : IAdminBookingRepository
    {
        private readonly AppDbContext _context; // غير AppDbContext لاسم الـ DbContext بتاعك

        public AdminBookingRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Activity Bookings ──

        public async Task<List<ActivityBookingDto>> GetAllActivityBookingsAsync()
        {
            return await _context.ActivityBookings
                .Include(b => b.Activity)
                .Include(b => b.ActivityGroup)
                    .ThenInclude(g => g.TimeSlots)
                .Include(b => b.Member)
                .Include(b => b.Trainer)  // ← لازم يكون هنا قبل الـ Select
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new ActivityBookingDto
                {
                    Id = b.Id,
                    ActivityId = b.ActivityId,
                    ActivityName = b.Activity.Name,
                    ActivityGroupId = b.ActivityGroupId,
                    GroupName = b.ActivityGroup.Name,
                    MemberId = b.MemberId,
                    MemberShipNumber = b.Member != null ? b.Member.MemberShipNumber.ToString() : "",
                    MemberName = b.Member != null ? b.Member.FullName : (b.Guest != null ? b.Guest.FullName : ""),
                    MemberEmail = b.Member != null ? b.Member.Email : (b.Guest != null ? b.Guest.Email : ""),
                    TrainerId = b.TrainerId,
                    // ← الفرق هنا: EF Core في الـ Select بيحتاج nullable check صريح
                    TrainerName = b.TrainerId != null
                        ? b.Trainer.FullName
                        : "No Trainer Assigned",
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Participants = b.Participants,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    PaymentMethod = b.PaymentMethod,
                    TransactionId = b.TransactionId,
                    ReceiptImageUrl = b.ReceiptImageUrl,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }
        public async Task<ActivityBookingDto?> GetActivityBookingByIdAsync(int id)
        {
            return await _context.ActivityBookings
                .Include(b => b.Activity)
                .Include(b => b.ActivityGroup)
                .Include(b => b.Member)
                .Where(b => b.Id == id)
                .Select(b => new ActivityBookingDto
                {
                    Id = b.Id,
                    ActivityId = b.ActivityId,
                    ActivityName = b.Activity.Name,
                    ActivityGroupId = b.ActivityGroupId,
                    GroupName = b.ActivityGroup.Name,
                    MemberId = b.MemberId,
                    MemberName = b.Member != null ? b.Member.FullName : (b.Guest != null ? b.Guest.FullName : ""),
                    MemberEmail = b.Member != null ? b.Member.Email : (b.Guest != null ? b.Guest.Email : ""),
                    TrainerId = b.TrainerId,
                    TrainerName = b.Trainer != null ? b.Trainer.FullName : "No Trainer Assigned",
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Participants = b.Participants,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                    PaymentMethod = b.PaymentMethod,
                    TransactionId = b.TransactionId,
                    ReceiptImageUrl = b.ReceiptImageUrl,
                    CreatedAt = b.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateActivityBookingStatusAsync(int id, string status)
        {
            var booking = await _context.ActivityBookings.FindAsync(id);
            if (booking == null) return false;

            booking.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Facility Bookings ──

        public async Task<List<AdminFacilityBookingDto>> GetAllFacilityBookingsAsync()
        {
            return await _context.FacilityBookings
                .Include(b => b.Facility)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new AdminFacilityBookingDto
                {
                    Id = b.Id,
                    FacilityName = b.Facility.Name,
                    BookedByName = b.BookedByName,
                    BookedByEmail = b.BookedByEmail,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Participants = b.Participants,
                    Price = b.Price,
                    Status = b.Status,
                    PaymentMethod = b.PaymentMethod,
                    TransactionId = b.TransactionId,
                    ReceiptImageUrl = b.ReceiptImageUrl,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<AdminFacilityBookingDto?> GetFacilityBookingByIdAsync(int id)
        {
            return await _context.FacilityBookings
                .Include(b => b.Facility)
                .Where(b => b.Id == id)
                .Select(b => new AdminFacilityBookingDto
                {
                    Id = b.Id,
                    FacilityName = b.Facility.Name,
                    BookedByName = b.BookedByName,
                    BookedByEmail = b.BookedByEmail,
                    BookingDate = b.BookingDate,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Participants = b.Participants,
                    Price = b.Price,
                    Status = b.Status,
                    PaymentMethod = b.PaymentMethod,
                    TransactionId = b.TransactionId,
                    ReceiptImageUrl = b.ReceiptImageUrl,
                    CreatedAt = b.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateFacilityBookingStatusAsync(int id, string status)
        {
            var booking = await _context.FacilityBookings.FindAsync(id);
            if (booking == null) return false;

            booking.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
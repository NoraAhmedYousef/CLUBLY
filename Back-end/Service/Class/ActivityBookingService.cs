using Clubly.DTO;
using Clubly.Model;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Service.Class
{
    public class ActivityBookingService: IActivityBookingService
    {
        private readonly IActivityBookingRepository _repo;
        private readonly AppDbContext _db;

        public ActivityBookingService(IActivityBookingRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        private static ActivityBookingDto ToDto(ActivityBooking b) => new()
        {
            Id = b.Id,
            ActivityId = b.ActivityId,
            ActivityName = b.Activity?.Name ?? "",
            ActivityGroupId = b.ActivityGroupId,
            GroupName = b.ActivityGroup?.Name ?? "",
            MemberId = b.MemberId,
            MemberName = b.Member?.FullName ?? "",
            MemberEmail = b.Member?.Email ?? "",
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Participants = b.Participants,
            TotalPrice = b.TotalPrice,
            Status = b.Status,
            PaymentMethod = b.PaymentMethod,
            TransactionId = b.TransactionId,
            CreatedAt = b.CreatedAt,
        };

        public async Task<List<ActivityBookingDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToDto).ToList();

        public async Task<List<ActivityBookingDto>> GetByActivityAsync(int activityId) =>
            (await _repo.GetByActivityAsync(activityId)).Select(ToDto).ToList();

        public async Task<List<ActivityBookingDto>> GetByMemberAsync(int memberId) =>
            (await _repo.GetByMemberAsync(memberId)).Select(ToDto).ToList();

        public async Task<ActivityBookingDto?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            return b is null ? null : ToDto(b);
        }

        public async Task<(ActivityBookingDto? result, string? error)> CreateAsync(CreateActivityBookingDto dto)
        {
            var group = await _db.ActivityGroups
                                 .FirstOrDefaultAsync(g => g.Id == dto.ActivityGroupId &&
                                                           g.ActivityId == dto.ActivityId);
            if (group is null)
                return (null, "Activity group not found.");

            var isDuplicate = await _repo.IsDuplicateAsync(dto.MemberId, dto.ActivityGroupId);
            if (isDuplicate)
                return (null, "You already have an active booking in this group.");

            var endDate = group.DurationDays.HasValue
                ? dto.StartDate.AddDays(group.DurationDays.Value)
                : dto.StartDate.AddDays(30);

            var booking = new ActivityBooking
            {
                ActivityId = dto.ActivityId,
                ActivityGroupId = dto.ActivityGroupId,
                MemberId = dto.MemberId,
                StartDate = dto.StartDate,
                EndDate = endDate,
                Participants = dto.Participants,
                TotalPrice = group.Price * dto.Participants,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = dto.TransactionId,
                Status = "Pending",
            };

            var created = await _repo.CreateAsync(booking);
            var result = await _repo.GetByIdAsync(created.Id);
            return (ToDto(result!), null);
        }

        public async Task<(bool success, string? error)> UpdateStatusAsync(int id, UpdateActivityBookingStatusDto dto)
        {
            var allowed = new[] { "Approved", "Cancelled" };
            if (!allowed.Contains(dto.Status))
                return (false, "Status must be 'Approved' or 'Cancelled'.");

            var booking = await _repo.GetByIdAsync(id);
            if (booking is null)
                return (false, "Booking not found.");

            if (booking.Status != "Pending")
                return (false, $"Booking is already '{booking.Status}'.");

            var ok = await _repo.UpdateStatusAsync(id, dto.Status);
            return ok ? (true, null) : (false, "Update failed.");
        }

        public async Task<bool> DeleteAsync(int id) =>
            await _repo.DeleteAsync(id);
    }
}
    

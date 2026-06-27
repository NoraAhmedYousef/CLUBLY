using Clubly.DTO;
using Clubly.Repository.Interfaces;
using Clubly.Service.Interfaces;

namespace Clubly.Service.Class
{
    public class AdminBookingService : IAdminBookingService
    {
        private readonly IAdminBookingRepository _repo;

        public AdminBookingService(IAdminBookingRepository repo)
        {
            _repo = repo;
        }

        private static readonly string[] AllowedStatuses = { "Approved", "Cancelled", "Pending" };

        // ── Activity ──

        public Task<List<ActivityBookingDto>> GetAllActivityBookingsAsync()
            => _repo.GetAllActivityBookingsAsync();

        public Task<ActivityBookingDto?> GetActivityBookingByIdAsync(int id)
            => _repo.GetActivityBookingByIdAsync(id);

        public async Task<(bool success, string? error)> UpdateActivityBookingStatusAsync(int id, string status)
        {
            if (!AllowedStatuses.Contains(status))
                return (false, $"Status غير صالح. المسموح: {string.Join(", ", AllowedStatuses)}");

            var ok = await _repo.UpdateActivityBookingStatusAsync(id, status);
            return ok ? (true, null) : (false, "Booking مش موجود.");
        }

        // ── Facility ──

        public Task<List<AdminFacilityBookingDto>> GetAllFacilityBookingsAsync()
            => _repo.GetAllFacilityBookingsAsync();

        public Task<AdminFacilityBookingDto?> GetFacilityBookingByIdAsync(int id)
            => _repo.GetFacilityBookingByIdAsync(id);

        public async Task<(bool success, string? error)> UpdateFacilityBookingStatusAsync(int id, string status)
        {
            if (!AllowedStatuses.Contains(status))
                return (false, $"Status غير صالح. المسموح: {string.Join(", ", AllowedStatuses)}");

            var ok = await _repo.UpdateFacilityBookingStatusAsync(id, status);
            return ok ? (true, null) : (false, "Booking مش موجود.");
        }
    }
}


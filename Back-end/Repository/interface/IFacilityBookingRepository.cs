using SignUp.Model;

namespace Clubly.Repository.Interface
{
    public interface IFacilityBookingRepository
    {
        Task<List<FacilityBooking>> GetAllAsync();
        Task<FacilityBooking?> GetByIdAsync(int id);
        Task<List<FacilityBooking>> GetByFacilityAsync(int facilityId);
        Task<bool> HasConflictAsync(int facilityId, DateOnly date, TimeOnly start, TimeOnly end, int? excludeId = null);
        Task<FacilityBooking> CreateAsync(FacilityBooking booking);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }
}

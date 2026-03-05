using Clubly.DTO;
using Clubly.Repository.Interface;
using Clubly.Service.Interfaces;
using SignUp.Model;

namespace Clubly.Service.Class
{
    public class FacilityBookingService: IFacilityBookingService
    {
        private readonly IFacilityBookingRepository _repo;
        public FacilityBookingService(IFacilityBookingRepository repo) => _repo = repo;

        private static FacilityBookingDto ToDto(FacilityBooking b) => new()
        {
            Id = b.Id,
            FacilityId = b.FacilityId,
            FacilityName = b.Facility?.Name ?? "",
            ScheduleId = b.FacilityScheduleId,
            MemberId = b.MemberId,
            BookedByName = b.BookedByName,
            BookedByEmail = b.BookedByEmail,
            BookingDate = b.BookingDate,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Participants = b.Participants,
            Status = b.Status,
            PaymentMethod = b.PaymentMethod,
            TransactionId = b.TransactionId,
            Price = b.Price,
            CreatedAt = b.CreatedAt,
        };

        public async Task<List<FacilityBookingDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToDto).ToList();

        public async Task<FacilityBookingDto?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            return b is null ? null : ToDto(b);
        }

        public async Task<(FacilityBookingDto? result, string? error)> CreateAsync(CreateFacilityBookingDto dto)
        {
            // Conflict check
            var conflict = await _repo.HasConflictAsync(
                dto.FacilityId, dto.BookingDate, dto.StartTime, dto.EndTime);

            if (conflict)
                return (null, "This time slot is already booked for this facility.");

            var booking = new FacilityBooking
            {
                FacilityId = dto.FacilityId,
                FacilityScheduleId = dto.FacilityScheduleId,
                MemberId = dto.MemberId,
                BookedByName = dto.BookedByName,
                BookedByEmail = dto.BookedByEmail,
                BookingDate = dto.BookingDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Participants = dto.Participants,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = dto.TransactionId,
                Price = dto.Price,
                Status = "Pending",
            };

            var created = await _repo.CreateAsync(booking);
            var result = await _repo.GetByIdAsync(created.Id);
            return (ToDto(result!), null);
        }

        public async Task<bool> UpdateStatusAsync(int id, UpdateFacilityBookingStatusDto dto)
        {
            if (await _repo.GetByIdAsync(id) is null) return false;
            await _repo.UpdateStatusAsync(id, dto.Status);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repo.GetByIdAsync(id) is null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
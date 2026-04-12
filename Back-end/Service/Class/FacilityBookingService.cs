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
            ScheduleId = b.FacilityScheduleId ?? 0,
            MemberId = b.MemberId,
            GuestId = b.GuestId,

            MemberShipNumber = b.Member?.MemberShipNumber.ToString() ?? "",

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
ReceiptImageUrl = b.ReceiptImageUrl ?? "",
CreatedAt = b.CreatedAt,
        };

        public async Task<List<FacilityBookingDto>> GetAllAsync() =>
            (await _repo.GetAllAsync()).Select(ToDto).ToList();

        public async Task<FacilityBookingDto?> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            return b is null ? null : ToDto(b);
        }
        private static async Task<string?> SaveReceiptAsync(string? base64)
        {
            if (string.IsNullOrEmpty(base64) || !base64.StartsWith("data:image"))
                return base64;

            var data = base64.Split(',')[1];
            var bytes = Convert.FromBase64String(data);
            var uploads = Path.Combine("wwwroot", "receipts");
            Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid()}.jpg";
            await File.WriteAllBytesAsync(Path.Combine(uploads, fileName), bytes);
            return $"/receipts/{fileName}";
        }
        public async Task<(FacilityBookingDto? result, string? error)> CreateAsync(CreateFacilityBookingDto dto)
        {
            // Conflict check
            if (dto.FacilityScheduleId.HasValue && dto.FacilityScheduleId > 0)
            {
                var conflict = await _repo.HasConflictAsync(
                    dto.FacilityId, DateOnly.Parse(dto.BookingDate),
    TimeOnly.Parse(dto.StartTime),
    TimeOnly.Parse(dto.EndTime));

                if (conflict)
                    return (null, "This time slot is already booked for this facility.");
            }

            var booking = new FacilityBooking
            {
                FacilityId = dto.FacilityId,
                FacilityScheduleId = dto.FacilityScheduleId ?? 0,
                MemberId = dto.MemberId.HasValue && dto.MemberId > 0 ? dto.MemberId : null,
                GuestId = dto.GuestId.HasValue && dto.GuestId > 0 ? dto.GuestId : null,
                BookedByName = dto.BookedByName,
                BookedByEmail = dto.BookedByEmail,
                BookingDate = DateOnly.Parse(dto.BookingDate),
                StartTime = TimeOnly.Parse(dto.StartTime),
                EndTime = TimeOnly.Parse(dto.EndTime),
                Participants = dto.Participants,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = dto.TransactionId,

                Price = dto.Price,
                ReceiptImageUrl = await SaveReceiptAsync(dto.ReceiptImageUrl),
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
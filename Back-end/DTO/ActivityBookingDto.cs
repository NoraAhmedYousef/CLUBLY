using Clubly.Model;

namespace Clubly.DTO
{
    public class ActivityBookingDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = "";
        public int ActivityGroupId { get; set; }
        public string GroupName { get; set; } = "";
        public int? MemberId { get; set; }
        public string? MemberName { get; set; } = "";
        public string? GuestName { get; set; } = "";

        public int? GuestId { get; set; }
        public int? TrainerId { get; set; }
        public string? TrainerName { get; set; }
        public string MemberShipNumber { get; set; } = "";

        public string MemberEmail { get; set; } = "";
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<ActivityTimeSlotDto> TimeSlots { get; set; } = new();

        public int Participants { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public string ReceiptImageUrl { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}

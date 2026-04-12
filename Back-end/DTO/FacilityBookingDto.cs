namespace Clubly.DTO
{
    public class FacilityBookingDto
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = "";
        public int ScheduleId { get; set; }
        public int? MemberId { get; set; }
        public int? GuestId { get; set; }

        public string MemberShipNumber { get; set; } = "";

        public string BookedByName { get; set; } = "";
        public string BookedByEmail { get; set; } = "";
        public DateOnly BookingDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Participants { get; set; }
        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public decimal Price { get; set; }
        public string ReceiptImageUrl { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}

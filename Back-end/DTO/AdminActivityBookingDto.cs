namespace Clubly.DTO
{
    public class AdminActivityBookingDto
    {
        public int Id { get; set; }
        public string ActivityName { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string MemberEmail { get; set; } = "";
        public string? TrainerName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Participants { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";

        public string ReceiptImageUrl { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}

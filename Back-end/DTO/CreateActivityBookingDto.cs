namespace Clubly.DTO
{
    public class CreateActivityBookingDto
    {
        public int ActivityId { get; set; }
        public int ActivityGroupId { get; set; }
        public int? MemberId { get; set; }
        public int? GuestId { get; set; }
        public int? TrainerId { get; set; }
        public DateOnly StartDate { get; set; }
        public int Participants { get; set; } = 1;
        public string PaymentMethod { get; set; } = "";
        public string ? ReceiptImageUrl { get; set; } = "";

        public string TransactionId { get; set; } = "";
    }
}

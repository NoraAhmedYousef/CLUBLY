namespace Clubly.DTO
{
    public class MembershipRenewalRequestDto
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public int MemberShipNumber { get; set; }

        public int MemberShipId { get; set; }
        public string MemberShipName { get; set; } = "";
        public int MemberShipDuration { get; set; }   // in days

        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public decimal Price { get; set; }
        public string ReceiptImageUrl { get; set; } = "";

        public DateTime RequestDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}

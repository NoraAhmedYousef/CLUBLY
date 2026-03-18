using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class CreateFacilityBookingDto
    {
        public int? FacilityScheduleId { get; set; }

        public int? MemberId { get; set; }

        public string BookedByName { get; set; } = "";

        public string BookedByEmail { get; set; } = "";

        [Required]
        public string BookingDate { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";

        [Range(1, 500)]
        public int Participants { get; set; } = 1;

        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public string? ReceiptImageUrl { get; set; } = "";

        public decimal Price { get; set; }
        public int FacilityId { get;  set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class CreateFacilityBookingDto
    {
        [Required]
        public int? FacilityScheduleId { get; set; }

        public int? MemberId { get; set; }

        [Required, MaxLength(100)]
        public string BookedByName { get; set; } = "";

        [MaxLength(100)]
        public string BookedByEmail { get; set; } = "";

        [Required]
        public DateOnly BookingDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Range(1, 500)]
        public int Participants { get; set; } = 1;

        public string PaymentMethod { get; set; } = "";
        public string TransactionId { get; set; } = "";
        public string ReceiptImageUrl { get; set; } = "";

        public decimal Price { get; set; }
        public int FacilityId { get;  set; }
    }
}

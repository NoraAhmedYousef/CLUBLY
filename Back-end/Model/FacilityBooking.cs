using Clubly.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class FacilityBooking
    {
        public int Id { get; set; }

        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        public int FacilityScheduleId { get; set; }
        public FacilitySchedule Schedule { get; set; } = null!;

        // Member أو Guest
        public int? MemberId { get; set; }

        [Required, MaxLength(100)]
        public string BookedByName { get; set; } = "";

        [MaxLength(100)]
        public string BookedByEmail { get; set; } = "";

        public DateOnly BookingDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int Participants { get; set; } = 1;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending / Confirmed / Cancelled

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = ""; // InstaPay / EWallet

        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(300)]
        public string ReceiptImageUrl { get; set; } = "";
    }
}

using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class ActivityBooking
    {

        public int Id { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        public int ActivityGroupId { get; set; }
        public ActivityGroup ActivityGroup { get; set; } = null!;

        public int? MemberId { get; set; }
        public Member? Member { get; set; }

        public int? GuestId { get; set; }
        public Guest? Guest { get; set; }

        public string BookedByName { get; set; } = "";
        public string BookedByEmail { get; set; } = "";
        public int? TrainerId { get; set; }                   
        public Trainer? Trainer { get; set; }                  
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public int Participants { get; set; } = 1;
        public decimal TotalPrice { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "";

        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ReceiptImageUrl { get; set; } = "";

    }
}

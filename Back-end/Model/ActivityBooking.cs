using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class ActivityBooking : IValidatableObject
    {

        [Key]
        public int Id { get; set; }

        // ── Activity ─────────────────────────────────
        [Required(ErrorMessage = "Activity is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityId must be greater than zero.")]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;

        // ── Activity Group ────────────────────────────
        [Required(ErrorMessage = "Activity group is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityGroupId must be greater than zero.")]
        public int ActivityGroupId { get; set; }
        public ActivityGroup ActivityGroup { get; set; } = null!;

        // ── Member / Guest (at least one required — use custom validation) ──
        [Range(1, int.MaxValue,
            ErrorMessage = "MemberId must be greater than zero.")]
        public int? MemberId { get; set; }
        public Member? Member { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "GuestId must be greater than zero.")]
        public int? GuestId { get; set; }
        public Guest? Guest { get; set; }

        // ── Booked By ─────────────────────────────────
        [Required(ErrorMessage = "Booked-by name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string BookedByName { get; set; } = "";

        [Required(ErrorMessage = "Booked-by email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150,
            ErrorMessage = "Email cannot exceed 150 characters.")]
        public string BookedByEmail { get; set; } = "";

        // ── Trainer (optional) ───────────────────────
        [Range(1, int.MaxValue,
            ErrorMessage = "TrainerId must be greater than zero.")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // ── Dates ─────────────────────────────────────
        [Required(ErrorMessage = "Start date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateOnly EndDate { get; set; }

        // ── Booking Details ───────────────────────────
        [Range(1, 100,
            ErrorMessage = "Participants must be between 1 and 100.")]
        public int Participants { get; set; } = 1;

        [Range(0, 999999.99,
            ErrorMessage = "Total price must be between 0 and 999,999.99.")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Pending|Confirmed|Cancelled|Completed)$",
            ErrorMessage = "Status must be: Pending, Confirmed, Cancelled, or Completed.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [RegularExpression(@"^(Cash|Card|BankTransfer|Online|)$",
            ErrorMessage = "Invalid payment method.")]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "";

        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Url(ErrorMessage = "Receipt image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Receipt URL cannot exceed 2048 characters.")]
        public string ReceiptImageUrl { get; set; } = "";

        public ICollection<Attendance> Attendances { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (EndDate < StartDate)
                yield return new ValidationResult(
                    "End date must be on or after start date.",
                    new[] { nameof(EndDate) });

            if (MemberId == null && GuestId == null)
                yield return new ValidationResult(
                    "Either a Member or a Guest must be assigned to the booking.",
                    new[] { nameof(MemberId), nameof(GuestId) });

            if (MemberId != null && GuestId != null)
                yield return new ValidationResult(
                    "A booking cannot have both a Member and a Guest.",
                    new[] { nameof(MemberId), nameof(GuestId) });
        }
        }
}

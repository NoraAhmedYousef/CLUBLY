using Clubly.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class FacilityBooking : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Facility ──────────────────────────────────
        [Required(ErrorMessage = "Facility is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "FacilityId must be greater than zero.")]
        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        // ── Schedule (optional) ───────────────────────
        [Range(1, int.MaxValue,
            ErrorMessage = "FacilityScheduleId must be greater than zero.")]
        public int? FacilityScheduleId { get; set; }
        public FacilitySchedule? Schedule { get; set; }

        // ── Member / Guest (exactly one required) ────
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

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100,
            ErrorMessage = "Email cannot exceed 100 characters.")]
        public string BookedByEmail { get; set; } = "";

        // ── Date & Time ───────────────────────────────
        [Required(ErrorMessage = "Booking date is required.")]
        public DateOnly BookingDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        // ── Participants ──────────────────────────────
        [Range(1, 10000,
            ErrorMessage = "Participants must be between 1 and 10,000.")]
        public int Participants { get; set; } = 1;

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Pending|Confirmed|Cancelled)$",
            ErrorMessage = "Status must be: Pending, Confirmed, or Cancelled.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        // ── Payment ───────────────────────────────────
        [RegularExpression(@"^(InstaPay|EWallet|Cash|Card|)$",
            ErrorMessage = "Invalid payment method.")]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "";

        [MaxLength(100)]
        public string TransactionId { get; set; } = "";

        [Range(0, 999999.99,
            ErrorMessage = "Price must be between 0 and 999,999.99.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Url(ErrorMessage = "Receipt image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Receipt URL cannot exceed 2048 characters.")]
        public string ReceiptImageUrl { get; set; } = "";

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // Exactly one of Member / Guest must be set
            if (MemberId == null && GuestId == null)
                yield return new ValidationResult(
                    "Either a Member or a Guest must be assigned to the booking.",
                    new[] { nameof(MemberId), nameof(GuestId) });

            if (MemberId != null && GuestId != null)
                yield return new ValidationResult(
                    "A booking cannot have both a Member and a Guest.",
                    new[] { nameof(MemberId), nameof(GuestId) });

            // EndTime must be after StartTime
            if (EndTime <= StartTime)
                yield return new ValidationResult(
                    "End time must be after start time.",
                    new[] { nameof(EndTime) });

            // Minimum slot duration: 30 minutes
            if (EndTime.ToTimeSpan() - StartTime.ToTimeSpan() < TimeSpan.FromMinutes(30))
                yield return new ValidationResult(
                    "Booking duration must be at least 30 minutes.",
                    new[] { nameof(EndTime) });

            // BookingDate must not be in the past
            if (BookingDate < DateOnly.FromDateTime(DateTime.UtcNow.Date))
                yield return new ValidationResult(
                    "Booking date cannot be in the past.",
                    new[] { nameof(BookingDate) });
        }
    }
}
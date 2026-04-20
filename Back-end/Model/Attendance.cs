using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class Attendance : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Booking ───────────────────────────────────
        [Required(ErrorMessage = "Activity booking is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityBookingId must be greater than zero.")]
        public int ActivityBookingId { get; set; }

        [Required]
        public ActivityBooking ActivityBooking { get; set; }

        // ── Attendance Date ───────────────────────────
        [Required(ErrorMessage = "Attendance date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        // ── Presence ──────────────────────────────────
        [Required]
        public bool IsPresent { get; set; }

        // ── Notes ─────────────────────────────────────
        [StringLength(500,
            ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        // ── Audit ─────────────────────────────────────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (Date > DateTime.UtcNow)
                yield return new ValidationResult(
                    "Attendance date cannot be in the future.",
                    new[] { nameof(Date) });

            if (Date.Year < 2000)
                yield return new ValidationResult(
                    "Attendance date is not realistic.",
                    new[] { nameof(Date) });
        }
    }
}
using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class FacilitySchedule : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Facility ──────────────────────────────────
        [Required(ErrorMessage = "Facility is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "FacilityId must be greater than zero.")]
        public int FacilityId { get; set; }
        public Facility Facility { get; set; } = null!;

        // ── Day ───────────────────────────────────────
        [Required(ErrorMessage = "Day is required.")]
        [RegularExpression(
            @"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$",
            ErrorMessage = "Day must be a valid full day name (e.g. Monday).")]
        [MaxLength(20)]
        public string Day { get; set; } = "";

        // ── Date ──────────────────────────────────────
        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive|Closed|Holiday)$",
            ErrorMessage = "Status must be: Active, Inactive, Closed, or Holiday.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        // ── Navigation ────────────────────────────────
        public List<FacilityTimeSlot> TimeSlots { get; set; } = new();

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // Date must match the declared Day of week
            var expectedDay = Date.DayOfWeek.ToString(); // e.g. "Monday"
            if (!string.IsNullOrWhiteSpace(Day) &&
                !string.Equals(Day, expectedDay, StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult(
                    $"Day '{Day}' does not match the date {Date} which falls on a {expectedDay}.",
                    new[] { nameof(Day), nameof(Date) });
        }
    }
}
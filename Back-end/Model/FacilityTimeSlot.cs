using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class FacilityTimeSlot : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Schedule ──────────────────────────────────
        [Required(ErrorMessage = "Facility schedule is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "FacilityScheduleId must be greater than zero.")]
        public int FacilityScheduleId { get; set; }
        public FacilitySchedule Schedule { get; set; } = null!;

        // ── Time ──────────────────────────────────────
        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        // ── Price ─────────────────────────────────────
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 999999.99,
            ErrorMessage = "Price must be between 0 and 999,999.99.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (EndTime <= StartTime)
                yield return new ValidationResult(
                    "End time must be after start time.",
                    new[] { nameof(EndTime) });

            if ((EndTime.ToTimeSpan() - StartTime.ToTimeSpan()).TotalMinutes < 30)
                yield return new ValidationResult(
                    "Time slot duration must be at least 30 minutes.",
                    new[] { nameof(EndTime) });
        }
    }
}

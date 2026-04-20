using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class ActivityGroupTimeSlot : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Activity Group ────────────────────────────
        [Required(ErrorMessage = "Activity group is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityGroupId must be greater than zero.")]
        public int ActivityGroupId { get; set; }
        public ActivityGroup ActivityGroup { get; set; } = null!;

        // ── Time ──────────────────────────────────────
        [Required(ErrorMessage = "Start time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        // ── Day ───────────────────────────────────────
        [RegularExpression(
            @"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)$",
            ErrorMessage = "Day must be a valid full day name (e.g. Monday).")]
        [MaxLength(10)]
        public string? Day { get; set; }

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (EndTime <= StartTime)
                yield return new ValidationResult(
                    "End time must be after start time.",
                    new[] { nameof(EndTime) });

            if ((EndTime - StartTime).TotalMinutes < 15)
                yield return new ValidationResult(
                    "Time slot duration must be at least 15 minutes.",
                    new[] { nameof(EndTime) });

            if (StartTime.TotalHours >= 24 || EndTime.TotalHours > 24)
                yield return new ValidationResult(
                    "Start and end times must be within a single day (00:00 – 24:00).",
                    new[] { nameof(StartTime), nameof(EndTime) });
        }
    
}
}

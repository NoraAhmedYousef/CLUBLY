using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class TrainerRating : IValidatableObject
    {

        [Key]
        public int Id { get; set; }

        // ── Relations ─────────────────────────────────
        [Required(ErrorMessage = "TrainerId is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "TrainerId must be greater than zero.")]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        [Required(ErrorMessage = "MemberId is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "MemberId must be greater than zero.")]
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        [Required(ErrorMessage = "ActivityBookingId is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityBookingId must be greater than zero.")]
        public int ActivityBookingId { get; set; }
        public ActivityBooking ActivityBooking { get; set; } = null!;

        // ── Rating ────────────────────────────────────
        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5,
            ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(1000,
            ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }

        // ── Audit ─────────────────────────────────────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // Comment required if rating is 1 or 2
            if (Rating <= 2 && string.IsNullOrWhiteSpace(Comment))
                yield return new ValidationResult(
                    "A comment is required when the rating is 1 or 2.",
                    new[] { nameof(Comment) });

            // CreatedAt cannot be in the future
            if (CreatedAt > DateTime.UtcNow.AddMinutes(1))
                yield return new ValidationResult(
                    "CreatedAt cannot be in the future.",
                    new[] { nameof(CreatedAt) });
        }
    }
}

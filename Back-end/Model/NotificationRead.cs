
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class NotificationRead
    {
        [Key]
        public int Id { get; set; }

        // ── Notification ──────────────────────────────
        [Required(ErrorMessage = "NotificationId is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "NotificationId must be greater than zero.")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

        // ── Recipient (only one should have a value) ──
        [Range(1, int.MaxValue,
            ErrorMessage = "GuestId must be greater than zero.")]
        public int? GuestId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "MemberId must be greater than zero.")]
        public int? MemberId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "TrainerId must be greater than zero.")]
        public int? TrainerId { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "AdminId must be greater than zero.")]
        public int? AdminId { get; set; }

        // ── Audit ─────────────────────────────────────
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            var recipients = new[]
            {
            GuestId, MemberId, TrainerId, AdminId
        };

            var filledCount = recipients.Count(id => id.HasValue);

            // Exactly one recipient must be set
            if (filledCount == 0)
                yield return new ValidationResult(
                    "At least one recipient must be specified (GuestId, MemberId, TrainerId, or AdminId).",
                    new[] { nameof(GuestId), nameof(MemberId), nameof(TrainerId), nameof(AdminId) });

            if (filledCount > 1)
                yield return new ValidationResult(
                    "Only one recipient can be specified per notification read record.",
                    new[] { nameof(GuestId), nameof(MemberId), nameof(TrainerId), nameof(AdminId) });

            // ReadAt cannot be in the future
            if (ReadAt > DateTime.UtcNow.AddMinutes(1))
                yield return new ValidationResult(
                    "ReadAt cannot be in the future.",
                    new[] { nameof(ReadAt) });
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class Notification : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Content ───────────────────────────────────
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 200 characters.")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(2000, MinimumLength = 5,
            ErrorMessage = "Description must be between 5 and 2,000 characters.")]
        public string Description { get; set; } = "";

        // ── Timing ────────────────────────────────────
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Sent|Draft)$",
            ErrorMessage = "Status must be: Sent or Draft.")]
        [MaxLength(20)]
        public string Status { get; set; } = "Draft";

        // ── Audience ──────────────────────────────────
        public bool ToMembers { get; set; }
        public bool ToTrainers { get; set; }
        public bool ToAdmins { get; set; }
        public bool ToGuests { get; set; }

        // ── Type & Media ──────────────────────────────
        [RegularExpression(@"^(Info|Warning|Alert|Promotion)$",
            ErrorMessage = "Type must be: Info, Warning, Alert, or Promotion.")]
        [MaxLength(50)]
        public string? Type { get; set; } = "Info";

        [Url(ErrorMessage = "Image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string? ImageUrl { get; set; }

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // At least one audience must be selected
            if (!ToMembers && !ToTrainers && !ToAdmins && !ToGuests)
                yield return new ValidationResult(
                    "At least one audience must be selected (Members, Trainers, Admins, or Guests).",
                    new[] { nameof(ToMembers) });

            // SentAt cannot be in the future if Status is Sent
            if (Status == "Sent" && SentAt > DateTime.UtcNow.AddMinutes(5))
                yield return new ValidationResult(
                    "A notification marked as Sent cannot have a future SentAt date.",
                    new[] { nameof(SentAt) });
        }
    }
}
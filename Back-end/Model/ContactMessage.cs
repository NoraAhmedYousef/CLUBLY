using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class ContactMessage : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Sender identity ───────────────────────────
        [Range(1, int.MaxValue,
            ErrorMessage = "UserId must be greater than zero.")]
        public int? UserId { get; set; }

        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 150 characters.")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150,
            ErrorMessage = "Email cannot exceed 150 characters.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(100,
            ErrorMessage = "Phone cannot exceed 100 characters.")]
        public string? Phone { get; set; }

        // ── Message content ───────────────────────────
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(1000, MinimumLength = 10,
            ErrorMessage = "Message must be between 10 and 1000 characters.")]
        public string Message { get; set; } = "";

        [StringLength(100,
            ErrorMessage = "Topic cannot exceed 100 characters.")]
        public string? Topic { get; set; }

        // ── Role ──────────────────────────────────────
        [RegularExpression(@"^(Admin|Member|Trainer|Guest)$",
            ErrorMessage = "UserRole must be: Admin, Member, Trainer, or Guest.")]
        [MaxLength(50)]
        public string? UserRole { get; set; }

        // ── Audit ─────────────────────────────────────
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // Guest (no UserId) must provide Name and Email
            if (UserId == null)
            {
                if (string.IsNullOrWhiteSpace(Name))
                    yield return new ValidationResult(
                        "Name is required for unauthenticated users.",
                        new[] { nameof(Name) });

                if (string.IsNullOrWhiteSpace(Email))
                    yield return new ValidationResult(
                        "Email is required for unauthenticated users.",
                        new[] { nameof(Email) });
            }

            // Registered user must have a matching role
            if (UserId != null && string.IsNullOrWhiteSpace(UserRole))
                yield return new ValidationResult(
                    "UserRole is required for authenticated users.",
                    new[] { nameof(UserRole) });
        }
    }
}
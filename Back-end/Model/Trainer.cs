using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Trainer : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Full name must be between 2 and 200 characters.")]
        public string FullName { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150,
            ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20,
            ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "National ID must be between 5 and 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
            ErrorMessage = "National ID can only contain letters and numbers.")]
        public string? NationalId { get; set; }

        // ── Personal Info ─────────────────────────────
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [RegularExpression(@"^(Male|Female|Other)$",
            ErrorMessage = "Gender must be: Male, Female, or Other.")]
        [MaxLength(50)]
        public string? Gender { get; set; }

        [Url(ErrorMessage = "Image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string? ImageUrl { get; set; }

        // ── Experience ────────────────────────────────
        [Range(0, 60,
            ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        // ── Status & Audit ────────────────────────────
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Security ──────────────────────────────────
        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string PasswordSalt { get; set; } = "";

        // ── Navigation ────────────────────────────────
        public List<Activity> Activities { get; set; } = new();

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (DateOfBirth.HasValue)
            {
                // Cannot be in the future
                if (DateOfBirth.Value > DateTime.Today)
                    yield return new ValidationResult(
                        "Date of birth cannot be in the future.",
                        new[] { nameof(DateOfBirth) });

                var age = DateTime.Today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                    yield return new ValidationResult(
                        "Trainer must be at least 18 years old.",
                        new[] { nameof(DateOfBirth) });

                if (age > 120)
                    yield return new ValidationResult(
                        "Date of birth is not realistic.",
                        new[] { nameof(DateOfBirth) });

                // Experience cannot exceed possible working years
                var maxPossibleExperience = age - 16;
                if (YearsOfExperience > maxPossibleExperience)
                    yield return new ValidationResult(
                        $"Years of experience cannot exceed {maxPossibleExperience} based on date of birth.",
                        new[] { nameof(YearsOfExperience) });
            }
        }

        }
}

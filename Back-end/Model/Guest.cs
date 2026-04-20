using SignUp.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clubly.Model
{

    public class Guest : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Name ──────────────────────────────────────
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = "";

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // ── Contact ───────────────────────────────────
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150,
            ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20,
            ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string Phone { get; set; } = "";

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "National ID is required.")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "National ID must be between 5 and 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
            ErrorMessage = "National ID can only contain letters and numbers.")]
        public string NationalId { get; set; } = "";

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression(@"^(Male|Female|Other)$",
            ErrorMessage = "Gender must be: Male, Female, or Other.")]
        [MaxLength(10)]
        public string Gender { get; set; } = "";

        // ── Personal Info ─────────────────────────────
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        // ── Image ─────────────────────────────────────
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        // ── Security ──────────────────────────────────
        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string PasswordSalt { get; set; } = "";

        // ── Audit ─────────────────────────────────────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // ── Navigation ────────────────────────────────

        public ICollection<ActivityBooking> ActivityBookings { get; set; }
            = new List<ActivityBooking>();

        public ICollection<FacilityBooking> FacilityBookings { get; set; }  // ← أضفها
    = new List<FacilityBooking>();
        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            if (DateOfBirth.HasValue)
            {
                if (DateOfBirth.Value > DateTime.Today)
                    yield return new ValidationResult(
                        "Date of birth cannot be in the future.",
                        new[] { nameof(DateOfBirth) });

                var age = DateTime.Today.Year - DateOfBirth.Value.Year;
                if (DateOfBirth.Value.Date > DateTime.Today.AddYears(-age)) age--;

                if (age < 5)
                    yield return new ValidationResult(
                        "Guest must be at least 5 years old.",
                        new[] { nameof(DateOfBirth) });

                if (age > 120)
                    yield return new ValidationResult(
                        "Date of birth is not realistic.",
                        new[] { nameof(DateOfBirth) });
            }
        }
    }
    }


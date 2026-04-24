
using Clubly.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SignUp.Model
{
    public class Member : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = "";

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

        // ── Dates ─────────────────────────────────────
        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Join date is required.")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        // ── Membership ────────────────────────────────
        [Required(ErrorMessage = "Membership number is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "Membership number must be greater than zero.")]
        public int MemberShipNumber { get; set; }

        [Required(ErrorMessage = "Member type is required.")]
        [RegularExpression(@"^(Regular|VIP|Premium|Student|Senior)$",
            ErrorMessage = "MemberType must be: Regular, VIP, Premium, Student, or Senior.")]
        [MaxLength(50)]
        public string MemberType { get; set; } = "";

        [Range(1, int.MaxValue,
            ErrorMessage = "MemberShipId must be greater than zero.")]
        public int? MemberShipId { get; set; }
        public MemberShip? MemberShip { get; set; }

        // ── Media ─────────────────────────────────────
        [Url(ErrorMessage = "Image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // ── Security ──────────────────────────────────
        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public string PasswordSalt { get; set; } = "";
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

        // ── Navigation ────────────────────────────────
        public ICollection<ActivityBooking> ActivityBookings { get; set; }
            = new List<ActivityBooking>();

        // ── Cross-field validation ────────────────────
        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // BirthDate guards
            if (BirthDate > DateTime.Today)
                yield return new ValidationResult(
                    "Birth date cannot be in the future.",
                    new[] { nameof(BirthDate) });

            var age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate.Date > DateTime.Today.AddYears(-age)) age--;

            if (age < 5)
                yield return new ValidationResult(
                    "Member must be at least 5 years old.",
                    new[] { nameof(BirthDate) });

            if (age > 120)
                yield return new ValidationResult(
                    "Birth date is not realistic.",
                    new[] { nameof(BirthDate) });

            // JoinDate must not be before BirthDate
            if (JoinDate < BirthDate)
                yield return new ValidationResult(
                    "Join date cannot be before birth date.",
                    new[] { nameof(JoinDate) });

            // JoinDate must not be in the future
            if (JoinDate > DateTime.UtcNow)
                yield return new ValidationResult(
                    "Join date cannot be in the future.",
                    new[] { nameof(JoinDate) });
        }


    }
}

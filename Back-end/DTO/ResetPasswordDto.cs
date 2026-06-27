using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class ResetPasswordDto : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string Otp { get; set; } = "";
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != ConfirmPassword)
                yield return new ValidationResult(
                    "Passwords do not match.",
                    new[] { nameof(ConfirmPassword) });
        }
    }
}

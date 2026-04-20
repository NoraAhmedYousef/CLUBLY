using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2,
          ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain uppercase, lowercase, and a number.")]
        public string Password { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone cannot exceed 20 characters.")]
        public string? Phone { get; set; }

        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "National ID must be between 5 and 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
            ErrorMessage = "National ID can only contain letters and numbers.")]
        public string? NationalId { get; set; }

        [RegularExpression(@"^(Male|Female|Other)$",
            ErrorMessage = "Gender must be: Male, Female, or Other.")]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

    }
}
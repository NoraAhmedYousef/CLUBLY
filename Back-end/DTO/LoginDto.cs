using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(admin|member|trainer|guest)$",
            ErrorMessage = "Role must be: admin, member, trainer, or guest.")]
        public string Role { get; set; } = "";
    }
}


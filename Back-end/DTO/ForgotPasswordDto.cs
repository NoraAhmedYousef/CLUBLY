using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}

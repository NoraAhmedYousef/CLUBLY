using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class LoginDto
    {
        [Required][EmailAddress] public string Email { get; set; } = "";
        [Required] public string Password { get; set; } = "";
        [Required] public string Role { get; set; } = ""; // admin | member | trainer | guest
    }
}


using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class RegisterDto
    {
        [Required] public string FirstName { get; set; } = "";
        [Required] public string LastName { get; set; } = "";
        [Required][EmailAddress] public string Email { get; set; } = "";
        [Required][MinLength(6)] public string Password { get; set; } = "";
        public string? Phone { get; set; }
        public string? NationalId { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

    }
}
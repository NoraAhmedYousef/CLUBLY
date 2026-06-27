using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateAdminDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string NationalId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }
        public IFormFile? Image { get; set; }
        public string Password { get; set; }

    }
}

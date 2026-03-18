using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        
        public string Email { get; set; }

        public string Phone { get; set; }

        public string NationalId { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public string? ImageUrl { get; set; }
        public string PasswordHash { get; set; } = "";
        public string PasswordSalt { get; set; } = "";

    }
}

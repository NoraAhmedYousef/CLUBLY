using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = "";

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; } = "";

        public int YearsOfExperience { get; set; }

        [MaxLength(20)]
        public string? NationalId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }
        public List<Activity> Activities { get; set; } = new();
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageUrl { get; set; }

        public string PasswordHash { get; set; } = "";
        public string PasswordSalt { get; set; } = "";

    }
}

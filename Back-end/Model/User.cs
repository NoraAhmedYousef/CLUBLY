using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string FullName { get; set; } = "";

        [Required, MaxLength(150)]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";
      

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(20)]
        public string? NationalId { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpCreatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}

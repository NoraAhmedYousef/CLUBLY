using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class PasswordResetToken
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string Token { get; set; } = "";

        public DateTime ExpireAt { get; set; }
    }
}

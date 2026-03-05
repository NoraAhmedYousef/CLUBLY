using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public int? UserId { get; set; }   // لو المستخدم مسجل

        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }

        [Required, MaxLength(1000)]
        public string Message { get; set; } = "";

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}

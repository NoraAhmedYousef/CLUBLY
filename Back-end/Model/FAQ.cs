using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        // ── Content ───────────────────────────────────
        [Required(ErrorMessage = "Question is required.")]
        [StringLength(300, MinimumLength = 10,
            ErrorMessage = "Question must be between 10 and 300 characters.")]
        public string Question { get; set; } = "";

        [Required(ErrorMessage = "Answer is required.")]
        [StringLength(2000, MinimumLength = 10,
            ErrorMessage = "Answer must be between 10 and 2000 characters.")]
        public string Answer { get; set; } = "";

        // ── Display ───────────────────────────────────
        [Range(0, 10000,
            ErrorMessage = "Display order must be between 0 and 10,000.")]
        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // ── Audit ─────────────────────────────────────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
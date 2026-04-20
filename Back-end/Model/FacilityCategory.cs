using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class FacilityCategory
    {
        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 150 characters.")]
        public string Name { get; set; } = "";

        [StringLength(1000,
            ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive)$",
            ErrorMessage = "Status must be: Active or Inactive.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        // ── Audit ─────────────────────────────────────
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ── Navigation ────────────────────────────────
        public List<Facility> Facilities { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class MemberShip
    {
        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Membership name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = "";

        [StringLength(500,
            ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // ── Duration & Price ──────────────────────────
        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 3650,
            ErrorMessage = "Duration must be between 1 and 3,650 days.")]
        public int Duration { get; set; } // in days

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.0, 100000.0,
            ErrorMessage = "Price must be between 0 and 100,000.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive)$",
            ErrorMessage = "Status must be: Active or Inactive.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        // ── Navigation ────────────────────────────────
        public List<Member> Members { get; set; } = new();
    
}
}

using Clubly.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Facility
    {

        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Facility name is required.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 150 characters.")]
        public string Name { get; set; } = "";

        [StringLength(1000,
            ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        // ── Capacity ──────────────────────────────────
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 100000,
            ErrorMessage = "Capacity must be between 1 and 100,000.")]
        public int Capacity { get; set; }

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive|UnderMaintenance|Closed)$",
            ErrorMessage = "Status must be: Active, Inactive, UnderMaintenance, or Closed.")]
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        // ── Media ─────────────────────────────────────
        [Url(ErrorMessage = "Image URL is not valid.")]
        [StringLength(2048,
            ErrorMessage = "Image URL cannot exceed 2048 characters.")]
        public string? ImageUrl { get; set; }

        // ── Category (optional) ───────────────────────
        [Range(1, int.MaxValue,
            ErrorMessage = "FacilityCategoryId must be greater than zero.")]
        public int? FacilityCategoryId { get; set; }
        public FacilityCategory? FacilityCategory { get; set; }

        // ── Navigation ────────────────────────────────
        public List<Activity> Activities { get; set; } = new();
        public List<ActivityGroup> ActivityGroups { get; set; } = new();
        public List<FacilitySchedule> Schedules { get; set; } = new();
        public List<FacilityBooking> Bookings { get; set; } = new();
    


}
}

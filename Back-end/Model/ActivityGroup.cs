using Clubly.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class ActivityGroup
    {
        [Key]
        public int Id { get; set; }

        // ── Identity ──────────────────────────────────
        [Required(ErrorMessage = "Group name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(20,
            ErrorMessage = "Code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\-_]*$",
            ErrorMessage = "Code can only contain letters, numbers, hyphens, and underscores.")]
        public string? Code { get; set; }

        // ── Capacity ──────────────────────────────────
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 1000,
            ErrorMessage = "Capacity must be between 1 and 1000.")]
        public int Capacity { get; set; }

        // ── Activity ──────────────────────────────────
        [Required(ErrorMessage = "Activity is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "ActivityId must be greater than zero.")]
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        // ── Trainer (optional) ───────────────────────
        [Range(1, int.MaxValue,
            ErrorMessage = "TrainerId must be greater than zero.")]
        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        // ── Pricing ───────────────────────────────────
        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 999999.99,
            ErrorMessage = "Price must be between 0 and 999,999.99.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // ── Duration ──────────────────────────────────
        [Range(1, 3650,
            ErrorMessage = "Duration must be between 1 and 3650 days (10 years).")]
        public int? DurationDays { get; set; }

        // ── Status ────────────────────────────────────
        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression(@"^(Active|Inactive|Full|Cancelled)$",
            ErrorMessage = "Status must be: Active, Inactive, Full, or Cancelled.")]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        // ── Navigation ────────────────────────────────
        public ICollection<ActivityGroupTimeSlot> TimeSlots { get; set; }
            = new List<ActivityGroupTimeSlot>();

        public ICollection<ActivityBooking> Bookings { get; set; }
            = new List<ActivityBooking>();


    }
}

using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Activity
    {
      
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Activity name is required.")]
            [StringLength(100, MinimumLength = 3,
                ErrorMessage = "Name must be between 3 and 100 characters.")]
            public string Name { get; set; } = "";

            [StringLength(500,
                ErrorMessage = "Description cannot exceed 500 characters.")]
            public string? Description { get; set; }

            [Required(ErrorMessage = "Facility is required.")]
            [Range(1, int.MaxValue,
                ErrorMessage = "FacilityId must be greater than zero.")]
            public int FacilityId { get; set; }

            [Required]
            public Facility Facility { get; set; }

            [Required(ErrorMessage = "Status is required.")]
            [RegularExpression(@"^(Active|Inactive|Pending)$",
                ErrorMessage = "Status must be: Active, Inactive, or Pending.")]
            public string Status { get; set; } = "Active";

            [Url(ErrorMessage = "Image URL is not a valid URL.")]
            [StringLength(2048,
                ErrorMessage = "Image URL cannot exceed 2048 characters.")]
            public string? ImageUrl { get; set; }

            public List<ActivityGroup> ActivityGroups { get; set; } = new();

            public List<Trainer> Trainers { get; set; } = new List<Trainer>();
        } }

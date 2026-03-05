using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateFacilityCategoryDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = "";

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = "Active";
    }
}

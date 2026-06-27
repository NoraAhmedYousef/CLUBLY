using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateFacilityDto
    {
        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Required]
        public int FacilityCategoryId { get; set; }

        [Range(1, 20000)]
        public int Capacity { get; set; }

        public string Status { get; set; } = "Active";

        public IFormFile? Image { get; set; }


    }
}

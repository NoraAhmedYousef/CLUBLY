using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class CreateActivityDto
    {
        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required]
        public int FacilityId { get; set; }

        public string Status { get; set; } = "Active";

        public IFormFile? Image { get; set; }


    }
}

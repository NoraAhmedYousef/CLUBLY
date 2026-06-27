using System.ComponentModel.DataAnnotations;

namespace SignUp.DTO
{
    public class UpdateFacilityCategoryDto
    {
        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? Status { get; set; }
    }
}

namespace SignUp.DTO
{
    public class UpdateFacilityDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int? FacilityCategoryId { get; set; }
        public int? Capacity { get; set; }

        public string? Status { get; set; }

        public IFormFile? Image { get; set; }



    }
}

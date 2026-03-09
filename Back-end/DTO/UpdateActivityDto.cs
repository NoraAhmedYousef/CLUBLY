namespace SignUp.DTO
{
    public class UpdateActivityDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal? Price { get; set; }
        public int? FacilityId { get; set; }

        public string? Status { get; set; }

        public IFormFile? Image { get; set; }

    }
}

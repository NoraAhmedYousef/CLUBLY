namespace SignUp.DTO
{
    public class UpdateMemberShipDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
    }
}

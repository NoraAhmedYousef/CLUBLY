namespace SignUp.DTO
{
    public class ActivityForGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? FacilityId { get; set; }
        public string? FacilityName { get; set; }
    }
}

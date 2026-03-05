namespace SignUp.DTO
{
    public class UpdateActivityGroupDto
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Code { get; set; }
        public int? ActivityId { get; set; }
        public string? Duration { get; set; }

        public string? Day { get; set; } 
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Status { get; set; } = "Active";
    }
}

namespace SignUp.DTO
{
    public class ActivityBookingDataDto
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = "";
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<TrainerOptionDto> Trainers { get; set; } = new();
        public List<GroupOptionDto> Groups { get; set; } = new();
    }
}

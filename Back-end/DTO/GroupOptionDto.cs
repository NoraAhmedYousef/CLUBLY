namespace SignUp.DTO
{
    public class GroupOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Code { get; set; }
        public int Capacity { get; set; }
        public string Day { get; set; } = "";
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Duration { get; set; } = "";
        public string TimeSlot => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        public string Schedule => $"{Day} {TimeSlot}";
    }
}

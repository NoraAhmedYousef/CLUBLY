namespace Clubly.DTO
{
    public class CreateActivityTimeSlotDto
    {

        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string? Day { get; set; }
    }
}

namespace Clubly.DTO
{
    public class TimeSlotDto
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal Price { get; set; }

    }
}

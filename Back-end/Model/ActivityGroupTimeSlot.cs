using SignUp.Model;

namespace Clubly.Model
{
    public class ActivityGroupTimeSlot
    {
        public int Id { get; set; }
        public int ActivityGroupId { get; set; }
        public ActivityGroup ActivityGroup { get; set; } = null!;
        public DateOnly Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Day { get; set; }
    }
}

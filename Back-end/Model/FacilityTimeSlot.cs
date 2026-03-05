namespace Clubly.Model
{
    public class FacilityTimeSlot
    {
        public int Id { get; set; }

        public int FacilityScheduleId { get; set; }
        public FacilitySchedule Schedule { get; set; } = null!;

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}

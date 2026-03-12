using Clubly.Model;

namespace SignUp.Model
{
    public class ActivityGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public int Capacity { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public int? TrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        public decimal Price { get; set; }

        public int? DurationDays { get; set; }

        public string Status { get; set; } = "Active";
        public ICollection<ActivityGroupTimeSlot> TimeSlots { get; set; } = new List<ActivityGroupTimeSlot>();



    }
}

using Clubly.DTO;

namespace SignUp.DTO
{
    public class ActivityGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Code { get; set; }
        public int? TrainerId { get; set; }
        public string? TrainerName { get; set; }
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? FacilityName { get; set; }
        public string? Duration { get; set; }

        public string? Day { get; set; } = string.Empty;
     
        public string? Status { get; set; }
        public List<ActivityTimeSlotDto> TimeSlots { get; set; } = new();

    }
}
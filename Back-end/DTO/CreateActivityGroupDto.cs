using Clubly.DTO;

namespace SignUp.DTO
{
    public class CreateActivityGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Code { get; set; }
        public int ActivityId { get; set; }
        public int? TrainerId { get; set; }
        public string Duration { get; set; }
        public string Day { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public List<CreateActivityTimeSlotDto> TimeSlots { get; set; } = new();
    }
}

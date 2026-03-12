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
        public decimal Price { get; set; }
        public int? DurationDays { get; set; }

        public string Status { get; set; } = "Active";
        public List<CreateActivityTimeSlotDto> TimeSlots { get; set; } = new();

    }
}

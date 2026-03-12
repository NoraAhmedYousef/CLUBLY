using Clubly.DTO;

namespace SignUp.DTO
{
    public class UpdateActivityGroupDto
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public string? Code { get; set; }
        public int? ActivityId { get; set; }
        public int? TrainerId { get; set; }
     
        public string? Status { get; set; } = "Active";
        public List<CreateActivityTimeSlotDto>? TimeSlots { get; set; }
        public decimal? Price { get; set; }


    }
}

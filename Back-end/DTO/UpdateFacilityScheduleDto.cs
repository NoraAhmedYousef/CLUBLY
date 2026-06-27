namespace Clubly.DTO
{
    public class UpdateFacilityScheduleDto
    {
        public int? FacilityId { get; set; }
        public string? Day { get; set; }
        public DateOnly? Date { get; set; }
        public string? Status { get; set; }
        public List<CreateTimeSlotDto>? TimeSlots { get; set; }
    }
}

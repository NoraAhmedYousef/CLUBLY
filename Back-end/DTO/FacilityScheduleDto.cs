namespace Clubly.DTO
{
    public class FacilityScheduleDto
    {
        public int Id { get; set; }
        public int FacilityId { get; set; }
        public string FacilityName { get; set; } = "";
        public string Day { get; set; } = "";
        public DateOnly Date { get; set; }
        public string Status { get; set; } = "Active";
        public List<TimeSlotDto> TimeSlots { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class CreateFacilityScheduleDto
    {
        [Required]
        public int FacilityId { get; set; }

        [Required, MaxLength(20)]
        public string Day { get; set; } = "";

        [Required]
        public DateOnly Date { get; set; }

        public string Status { get; set; } = "Active";

        [Required, MinLength(1, ErrorMessage = "At least one time slot is required.")]
        public List<CreateTimeSlotDto> TimeSlots { get; set; } = new();
    }
}

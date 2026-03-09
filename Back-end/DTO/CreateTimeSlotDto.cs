using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class CreateTimeSlotDto
    {
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly EndTime { get; set; }
    }
}

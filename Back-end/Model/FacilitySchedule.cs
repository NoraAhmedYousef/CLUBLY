using SignUp.Model;
using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class FacilitySchedule
    {
            public int Id { get; set; }

            public int FacilityId { get; set; }
            public Facility Facility { get; set; } = null!;

            [Required, MaxLength(20)]
            public string Day { get; set; } = "";

            public DateOnly Date { get; set; }

            [Required, MaxLength(50)]
            public string Status { get; set; } = "Active";

            public List<FacilityTimeSlot> TimeSlots { get; set; } = new();
        }
}

using Clubly.Model;
using System.ComponentModel.DataAnnotations;

namespace SignUp.Model
{
    public class Facility
    {

        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public int Capacity { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Active";

        public string? ImageUrl { get; set; }

        public int? FacilityCategoryId { get; set; }
        public FacilityCategory? FacilityCategory { get; set; }

        public List<Activity> Activities { get; set; } = new();
        public List<ActivityGroup> ActivityGroups { get; set; } = new();
        public List<FacilitySchedule> Schedules { get; set; } = new();
        public List<FacilityBooking> Bookings { get; set; } = new();



    }
}

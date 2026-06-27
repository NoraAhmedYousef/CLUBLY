using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class UpdateFacilityBookingStatusDto
    {
        [Required, MaxLength(50)]
        public string Status { get; set; } = ""; // Confirmed / Cancelled
    }
}

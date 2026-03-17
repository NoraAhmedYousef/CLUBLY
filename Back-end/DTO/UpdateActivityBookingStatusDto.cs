using System.ComponentModel.DataAnnotations;

namespace Clubly.DTO
{
    public class UpdateActivityBookingStatusDto
    {
        [Required]
        public string Status { get; set; } = "";
    }
}

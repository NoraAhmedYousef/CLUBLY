using System.ComponentModel.DataAnnotations;

namespace Clubly.Model
{
    public class AttendanceRecord
    {
        [Required(ErrorMessage = "Booking ID is required.")]
        [Range(1, int.MaxValue,
            ErrorMessage = "BookingId must be greater than zero.")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Presence status is required.")]
        public bool IsPresent { get; set; }

        [StringLength(500,
            ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}

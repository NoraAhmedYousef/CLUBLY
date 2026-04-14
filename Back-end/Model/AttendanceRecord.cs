namespace Clubly.Model
{
    public class AttendanceRecord
    {
        public int BookingId { get; set; }
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
    }
}

namespace Clubly.Model
{
    public class Attendance
    {
        public int Id { get; set; }
        public int ActivityBookingId { get; set; }
        public ActivityBooking ActivityBooking { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

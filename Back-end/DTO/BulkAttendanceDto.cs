using Clubly.Model;

namespace Clubly.DTO
{
    public class BulkAttendanceDto
    {
        public string Date { get; set; } = "";
        public List<AttendanceRecord> Records { get; set; } = new();
    }
}

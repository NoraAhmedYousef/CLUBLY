using Clubly.DTO;
using Clubly.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AttendanceController(AppDbContext context) => _context = context;

        // GET: api/Attendance?groupId=1&date=2025-01-15
        [HttpGet]
        public async Task<IActionResult> GetAttendance([FromQuery] int groupId, [FromQuery] string date)
        {
            if (!DateOnly.TryParse(date, out var parsedDate))
                return BadRequest("Invalid date.");

            var bookings = await _context.ActivityBookings
                .Include(b => b.Member)
                .Include(b => b.Guest)
                .Include(b => b.Activity)
                .Include(b => b.ActivityGroup).ThenInclude(g => g.TimeSlots)
                .Include(b => b.Attendances)
                .Where(b =>
                    b.ActivityGroupId == groupId &&
                    b.Status == "Approved" &&
                    b.StartDate <= parsedDate &&
                    b.EndDate >= parsedDate)
                .ToListAsync();

            var result = bookings.Select(b => {
                var att = b.Attendances?.FirstOrDefault(a =>
                    DateOnly.FromDateTime(a.Date) == parsedDate);
                return new
                {
                    bookingId = b.Id,
                    name = b.MemberId != null ? b.Member?.FullName : (b.Guest?.FullName ?? b.BookedByName),
                    email = b.MemberId != null ? b.Member?.Email : b.BookedByEmail,
                    isGuest = b.GuestId != null,
                    activityName = b.Activity?.Name,
                    groupName = b.ActivityGroup?.Name,
                    startDate = b.StartDate,
                    endDate = b.EndDate,
                    timeSlots = b.ActivityGroup?.TimeSlots?.Select(s => new { s.Day, s.StartTime, s.EndTime }),
                    attendanceId = att?.Id,
                    isPresent = att?.IsPresent,
                    notes = att?.Notes,
                    attended = att != null
                };
            });
            return Ok(result);
        }

        // POST: api/Attendance/bulk
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkSave([FromBody] BulkAttendanceDto dto)
        {
            if (!DateOnly.TryParse(dto.Date, out var parsedDate))
                return BadRequest("Invalid date.");

            foreach (var item in dto.Records)
            {
                var existing = await _context.Attendances.FirstOrDefaultAsync(a =>
                    a.ActivityBookingId == item.BookingId &&
                    DateOnly.FromDateTime(a.Date) == parsedDate);

                if (existing != null)
                {
                    existing.IsPresent = item.IsPresent;
                    existing.Notes = item.Notes;
                }
                else
                {
                    _context.Attendances.Add(new Attendance
                    {
                        ActivityBookingId = item.BookingId,
                        Date = parsedDate.ToDateTime(TimeOnly.MinValue),
                        IsPresent = item.IsPresent,
                        Notes = item.Notes,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            await _context.SaveChangesAsync();
            return Ok(new { message = "Saved", count = dto.Records.Count });
        }

        // GET: api/Attendance/summary/{bookingId}
        [HttpGet("summary/{bookingId}")]
        public async Task<IActionResult> GetSummary(int bookingId)
        {
            var booking = await _context.ActivityBookings
                .Include(b => b.Member)
                .Include(b => b.Activity)
                .Include(b => b.ActivityGroup).ThenInclude(g => g.TimeSlots)
                .Include(b => b.Attendances)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return NotFound();

            var atts = booking.Attendances ?? new List<Attendance>();
            var present = atts.Count(a => a.IsPresent);
            var slots = booking.ActivityGroup?.TimeSlots?.ToList() ?? new();
            var total = (booking.EndDate.DayNumber - booking.StartDate.DayNumber) + 1;

            int expected = 0;
            for (int i = 0; i < total; i++)
            {
                var d = booking.StartDate.AddDays(i);
                if (slots.Any(s => s.Day == d.DayOfWeek.ToString())) expected++;
            }

            return Ok(new
            {
                bookingId = booking.Id,
                name = booking.Member?.FullName ?? booking.BookedByName,
                activityName = booking.Activity?.Name,
                startDate = booking.StartDate,
                endDate = booking.EndDate,
                totalPresent = present,
                totalAbsent = atts.Count(a => !a.IsPresent),
                expectedSessions = expected,
                attendanceRate = expected > 0 ? Math.Round((double)present / expected * 100, 1) : 0,
                records = atts.OrderBy(a => a.Date).Select(a => new {
                    date = DateOnly.FromDateTime(a.Date),
                    isPresent = a.IsPresent,
                    notes = a.Notes,
                    startTime = slots.FirstOrDefault(s => s.Day == DateOnly.FromDateTime(a.Date).DayOfWeek.ToString())?.StartTime.ToString(),
                    endTime = slots.FirstOrDefault(s => s.Day == DateOnly.FromDateTime(a.Date).DayOfWeek.ToString())?.EndTime.ToString()
                })
            });
        }

        // GET: api/Attendance/groups/{trainerId}
        [HttpGet("groups/{trainerId}")]
        public async Task<IActionResult> GetTrainerGroups(int trainerId)
        {
            var groups = await _context.ActivityGroups
                .Include(g => g.Activity)
                .Include(g => g.TimeSlots)
                .Where(g => g.TrainerId == trainerId && g.Status == "Active")
                .Select(g => new {
                    id = g.Id,
                    name = g.Name,
                    activityName = g.Activity.Name,
                    capacity = g.Capacity,
                    timeSlots = g.TimeSlots.Select(s => new { s.Day, s.StartTime, s.EndTime })
                }).ToListAsync();

            return Ok(groups);
        }
    }

}


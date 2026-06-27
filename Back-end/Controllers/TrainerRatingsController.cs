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
    public class TrainerRatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainerRatingsController(AppDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────
        // POST /api/TrainerRatings
        // ─────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainerRatingDto dto)
        {
            // Validation
            if (dto.Rating < 1 || dto.Rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            // منع التكرار — member مقدرش يعمل rate لنفس الـ booking أكتر من مرة
            var exists = await _context.TrainerRatings
                .AnyAsync(r => r.MemberId == dto.MemberId
                            && r.ActivityBookingId == dto.ActivityBookingId);

            if (exists)
                return Conflict("You have already rated this trainer for this booking.");

            // تأكد إن الـ trainer موجود
            var trainerExists = await _context.Trainers
                .AnyAsync(t => t.Id == dto.TrainerId);

            if (!trainerExists)
                return NotFound("Trainer not found.");

            var rating = new TrainerRating
            {
                TrainerId = dto.TrainerId,
                MemberId = dto.MemberId,
                ActivityBookingId = dto.ActivityBookingId,
                Rating = dto.Rating,
                Comment = dto.Comment?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.TrainerRatings.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Rating submitted successfully.",
                id = rating.Id,
                trainerId = rating.TrainerId,
                rating = rating.Rating,
                activityBookingId = rating.ActivityBookingId
            });
        }

        // ─────────────────────────────────────────────
        // GET /api/TrainerRatings/trainer/{trainerId}
        // ─────────────────────────────────────────────
        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetByTrainer(int trainerId)
        {
            var ratings = await _context.TrainerRatings
        .Where(r => r.TrainerId == trainerId)
        .Include(r => r.Member)
        .Include(r => r.ActivityBooking)
            .ThenInclude(b => b.ActivityGroup)
                .ThenInclude(g => g.Activity)
        .OrderByDescending(r => r.CreatedAt)
        .Select(r => new
        {
            r.Id,
            r.Rating,
            r.Comment,
            r.CreatedAt,
            r.ActivityBookingId,
            memberName = r.Member.FullName,
            activityName = r.ActivityBooking.ActivityGroup.Activity.Name,
            groupName = r.ActivityBooking.ActivityGroup.Name
        })
        .ToListAsync();

            if (!ratings.Any())
                return Ok(new
                {
                    trainerId = trainerId,
                    averageRating = 0.0,
                    totalRatings = 0,
                    ratings = new List<object>()
                });

            var average = ratings.Average(r => r.Rating);

            return Ok(new
            {
                trainerId = trainerId,
                averageRating = Math.Round(average, 1),
                totalRatings = ratings.Count,
                ratings = ratings
            });
        }

        // ─────────────────────────────────────────────
        // GET /api/TrainerRatings/check?memberId=&bookingId=
        // ─────────────────────────────────────────────
        [HttpGet("check")]
        public async Task<IActionResult> Check([FromQuery] int memberId,
                                               [FromQuery] int bookingId)
        {
            var existing = await _context.TrainerRatings
                .Where(r => r.MemberId == memberId
                         && r.ActivityBookingId == bookingId)
                .Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (existing == null)
                return Ok(new { hasRated = false });

            return Ok(new
            {
                hasRated = true,
                ratingId = existing.Id,
                rating = existing.Rating,
                comment = existing.Comment,
                createdAt = existing.CreatedAt
            });
        }
    }
}
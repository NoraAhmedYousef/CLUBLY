using Clubly.DTO;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
            => _service = service;

        // ─── CRUD ──────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var n = await _service.GetByIdAsync(id);
            return n == null ? NotFound() : Ok(n);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] NotificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] NotificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        // ─── Read Tracking ─────────────────────────────────────

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id, [FromBody] MarkReadDto dto)
        {
            var exists = await _service.GetByIdAsync(id);
            if (exists == null) return NotFound();
            await _service.MarkReadAsync(id, dto);
            return Ok();
        }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllRead([FromBody] MarkReadDto dto)
        {
            await _service.MarkAllReadAsync(dto);
            return Ok();
        }

        // ─── Per-Role Endpoints ────────────────────────────────

        [HttpGet("for-guest/{guestId}")]
        public async Task<IActionResult> GetForGuest(int guestId)
        {
            var createdAt = await _service.GetUserCreatedAtAsync("guest", guestId);
            var result = await _service.GetForRoleAsync("guest", guestId, createdAt);
            return Ok(result);
        }

        [HttpGet("for-member/{memberId}")]
        public async Task<IActionResult> GetForMember(int memberId)
        {
            var createdAt = await _service.GetUserCreatedAtAsync("member", memberId);
            var result = await _service.GetForRoleAsync("member", memberId, createdAt);
            return Ok(result);
        }

        [HttpGet("for-trainer/{trainerId}")]
        public async Task<IActionResult> GetForTrainer(int trainerId)
        {
            var createdAt = await _service.GetUserCreatedAtAsync("trainer", trainerId);
            var result = await _service.GetForRoleAsync("trainer", trainerId, createdAt);
            return Ok(result);
        }

        [HttpGet("for-admin/{adminId}")]
        public async Task<IActionResult> GetForAdmin(int adminId)
        {
            var createdAt = await _service.GetUserCreatedAtAsync("admin", adminId);
            var result = await _service.GetForRoleAsync("admin", adminId, createdAt);
            return Ok(result);
        }
    }
}
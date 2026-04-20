using Clubly.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;
using SignUp.Model;
using System.Security.Claims;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ContactMessagesController(AppDbContext db) => _db = db;

        // POST — للكل بدون auth
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Send([FromBody] CreateContactMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message) || dto.Message.Length < 5)
                return BadRequest("Message is too short.");

            var msg = new ContactMessage
            {
                Message = dto.Message.Trim(),
                Name = dto.Name?.Trim(),
                Email = dto.Email?.Trim(),
                Phone = dto.Phone?.Trim(),
                Topic = dto.Topic?.Trim(),
                SentAt = DateTime.UtcNow,
            };

            // لو مسجّل دخول — جيب بياناته من الـ Token
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                if (int.TryParse(userIdClaim, out int uid)) msg.UserId = uid;
                msg.UserRole = roleClaim;

                if (string.IsNullOrEmpty(msg.Name))
                    msg.Name = User.FindFirst("FullName")?.Value
                             ?? User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(msg.Email))
                    msg.Email = User.FindFirst(ClaimTypes.Email)?.Value;
            }

            _db.ContactMessages.Add(msg);
            await _db.SaveChangesAsync();
            return Ok(new { msg.Id, message = "Message sent successfully." });
        }

        // GET — للـ Admin فقط
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _db.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            return Ok(messages);
        }

        // GET /{id} — تعليم كـ مقروء تلقائياً
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var msg = await _db.ContactMessages.FindAsync(id);
            if (msg == null) return NotFound();
            if (!msg.IsRead) { msg.IsRead = true; await _db.SaveChangesAsync(); }
            return Ok(msg);
        }

        // PATCH /{id}/read
        [HttpPatch("{id}/read")]
        [Authorize]
        public async Task<IActionResult> MarkRead(int id)
        {
            var msg = await _db.ContactMessages.FindAsync(id);
            if (msg == null) return NotFound();
            msg.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok();
        }

        // DELETE /{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var msg = await _db.ContactMessages.FindAsync(id);
            if (msg == null) return NotFound();
            _db.ContactMessages.Remove(msg);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
using Clubly.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignUp.Data;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FAQsController(AppDbContext db) => _db = db;

        // GET — للكل (الـ FAQ page)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var faqs = await _db.FAQs
                .Where(f => f.IsActive)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.CreatedAt)
                .ToListAsync();
            return Ok(faqs);
        }

        // GET /api/FAQs/all — للـ Admin (يشوف المحذوف والمخفي)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var faqs = await _db.FAQs
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();
            return Ok(faqs);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] FAQ dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Question) || string.IsNullOrWhiteSpace(dto.Answer))
                return BadRequest("Question and Answer are required.");

            var faq = new FAQ
            {
                Question = dto.Question.Trim(),
                Answer = dto.Answer.Trim(),
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _db.FAQs.Add(faq);
            await _db.SaveChangesAsync();
            return Ok(faq);
        }

        // PUT
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] FAQ dto)
        {
            var faq = await _db.FAQs.FindAsync(id);
            if (faq == null) return NotFound();

            faq.Question = dto.Question.Trim();
            faq.Answer = dto.Answer.Trim();
            faq.DisplayOrder = dto.DisplayOrder;
            faq.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();
            return Ok(faq);
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var faq = await _db.FAQs.FindAsync(id);
            if (faq == null) return NotFound();
            _db.FAQs.Remove(faq);
            await _db.SaveChangesAsync();
            return Ok();
        }

        // PATCH /{id}/toggle — تفعيل/تعطيل
        [HttpPatch("{id}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Toggle(int id)
        {
            var faq = await _db.FAQs.FindAsync(id);
            if (faq == null) return NotFound();
            faq.IsActive = !faq.IsActive;
            await _db.SaveChangesAsync();
            return Ok(new { faq.IsActive });
        }
    }
}
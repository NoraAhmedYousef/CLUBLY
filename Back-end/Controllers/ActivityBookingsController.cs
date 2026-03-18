using Clubly.DTO;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityBookingsController : ControllerBase
    {

        private readonly IActivityBookingService _service;
        public ActivityBookingsController(IActivityBookingService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound(new { message = "Booking not found." }) : Ok(dto);
        }

        [HttpGet("by-activity/{activityId:int}")]
        public async Task<IActionResult> GetByActivity(int activityId) =>
            Ok(await _service.GetByActivityAsync(activityId));

        [HttpGet("by-member/{memberId:int}")]
        public async Task<IActionResult> GetByMember(int memberId) =>
            Ok(await _service.GetByMemberAsync(memberId));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActivityBookingDto dto, IFormFile? receiptImage)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (receiptImage != null && receiptImage.Length > 0)
            {
                var uploads = Path.Combine("wwwroot", "receipts");
                Directory.CreateDirectory(uploads);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(receiptImage.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await receiptImage.CopyToAsync(stream);
                dto.ReceiptImageUrl = $"/receipts/{fileName}";
            }

            var (result, error) = await _service.CreateAsync(dto);
            if (error is not null) return Conflict(new { message = error });
            return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
        }

        // Admin: Approve أو Cancel
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateActivityBookingStatusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (success, error) = await _service.UpdateStatusAsync(id, dto);
            if (!success)
                return error == "Booking not found."
                    ? NotFound(new { message = error })
                    : BadRequest(new { message = error });
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound(new { message = "Booking not found." });
        }
    }
}

using Clubly.DTO;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityBookingsController : ControllerBase
    {
        private readonly IFacilityBookingService _service;
        public FacilityBookingsController(IFacilityBookingService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFacilityBookingDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = string.Join(", ", errors) });
            }
            try
            {
                var (result, error) = await _service.CreateAsync(dto);
                if (error is not null)
                    return Conflict(new { message = error });
                return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }

            // Admin: تغيير الـ status (Confirmed / Cancelled)
            [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFacilityBookingStatusDto dto)
        {
            var ok = await _service.UpdateStatusAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
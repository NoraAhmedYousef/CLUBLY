using Clubly.DTO;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBookingsController : ControllerBase
    {
        private readonly IAdminBookingService _service;

        public AdminBookingsController(IAdminBookingService service)
        {
            _service = service;
        }

        // ══ Activity Bookings ══

        [HttpGet("activities")]
        public async Task<IActionResult> GetAllActivityBookings()
            => Ok(await _service.GetAllActivityBookingsAsync());

        [HttpGet("activities/{id:int}")]
        public async Task<IActionResult> GetActivityBookingById(int id)
        {
            var dto = await _service.GetActivityBookingByIdAsync(id);
            return dto is null ? NotFound(new { message = "Booking مش موجود." }) : Ok(dto);
        }

        [HttpPatch("activities/{id:int}/status")]
        public async Task<IActionResult> UpdateActivityStatus(int id, [FromBody] UpdateActivityBookingStatusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (success, error) = await _service.UpdateActivityBookingStatusAsync(id, dto.Status);
            if (!success)
                return error!.Contains("موجود") ? NotFound(new { message = error }) : BadRequest(new { message = error });
            return NoContent();
        }

        // ══ Facility Bookings ══

        [HttpGet("facilities")]
        public async Task<IActionResult> GetAllFacilityBookings()
            => Ok(await _service.GetAllFacilityBookingsAsync());

        [HttpGet("facilities/{id:int}")]
        public async Task<IActionResult> GetFacilityBookingById(int id)
        {
            var dto = await _service.GetFacilityBookingByIdAsync(id);
            return dto is null ? NotFound(new { message = "Booking مش موجود." }) : Ok(dto);
        }

        [HttpPatch("facilities/{id:int}/status")]
        public async Task<IActionResult> UpdateFacilityStatus(int id, [FromBody] UpdateFacilityBookingStatusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (success, error) = await _service.UpdateFacilityBookingStatusAsync(id, dto.Status);
            if (!success)
                return error!.Contains("موجود") ? NotFound(new { message = error }) : BadRequest(new { message = error });
            return NoContent();
        }
    }
}

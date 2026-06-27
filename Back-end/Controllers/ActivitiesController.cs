using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Class;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdDtoAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateActivityDto dto)
        {
            var activity = await _service.CreateAsync(dto);
            var result = await _service.GetByIdDtoAsync(activity.Id);

            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateActivityDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/booking-data")]
        public async Task<IActionResult> GetActivityBookingData(int id)
        {
            var data = await _service.GetActivityBookingDataAsync(id);

            if (data == null)
                return NotFound(new { message = "Activity not found or inactive" });

            return Ok(data);
        }
    }
    }


using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Class;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly ITrainerService _service;
        private readonly IActivityService _activityService;


        public TrainersController(ITrainerService service, IActivityService activityService)
        {
            _service = service;
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var trainer = await _service.GetByIdAsync(id);
            if (trainer == null) return NotFound();
            return Ok(trainer);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateTrainerDto dto)
        {
            var trainer = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = trainer.Id }, trainer);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateTrainerDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
        [HttpGet("activities")]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _activityService.GetAllForDash();
            return Ok(activities);
        }

    }
}

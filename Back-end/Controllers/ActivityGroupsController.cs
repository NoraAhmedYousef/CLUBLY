using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityGroupsController : ControllerBase
    {
        private readonly IActivityGroupService _service;
        public ActivityGroupsController(IActivityGroupService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var g = await _service.GetByIdAsync(id);
            return g == null ? NotFound() : Ok(g);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateActivityGroupDto dto)
        {
            var group = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = group.Id }, group);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateActivityGroupDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
        [HttpGet("byActivity/{activityId}")]
        public async Task<IActionResult> GetByActivity(int activityId)
        {
            return Ok(await _service.GetByActivityIdAsync(activityId));
        }
    }
}
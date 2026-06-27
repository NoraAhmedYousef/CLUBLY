using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Class;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly IFacilityService _service;

        public FacilitiesController(IFacilityService service)
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
        public async Task<IActionResult> Create([FromForm] CreateFacilityDto dto)
        {
            var facility = await _service.CreateAsync(dto);
            var result = await _service.GetByIdDtoAsync(facility.Id);

            return CreatedAtAction(nameof(GetById), new { id = facility.Id }, result);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateFacilityDto dto)
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

        [HttpGet("Categories")]
        public async Task<IActionResult> GetAllForDash()
        {
            return Ok(await _service.GetAllForDash());
        }

    }
}
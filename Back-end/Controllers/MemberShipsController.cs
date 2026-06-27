using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Class;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberShipsController : ControllerBase
    {
        private readonly IMemberShipService _service;

        public MemberShipsController(IMemberShipService service)
        {
            _service = service;
            
        }

        // GET: api/membership
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var memberships = await _service.GetAllAsync();
            return Ok(memberships);
        }

        // GET: api/membership/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var membership = await _service.GetByIdAsync(id);
            if (membership == null)
                return NotFound(new { message = "Membership not found" });

            return Ok(membership);
        }

        // POST: api/membership
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMemberShipDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        // PUT: api/membership/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberShipDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _service.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Membership not found" });

            return Ok(new { message = "Updated successfully" });
        }

        // DELETE: api/membership/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Membership not found" });

            return Ok(new { message = "Deleted successfully" });
        }
       
    }

}



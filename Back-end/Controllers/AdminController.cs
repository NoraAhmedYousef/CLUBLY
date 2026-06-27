using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Interfaces;

namespace SignUp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/admin
        [HttpGet]
        public async Task<ActionResult<List<AdminDto>>> GetAll()
        {
            var admins = await _adminService.GetAllAdminsAsync();
            return Ok(admins);
        }

        // GET: api/admin/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetById(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null) return NotFound("Admin not found");
            return Ok(admin);
        }

        // POST: api/admin
        [HttpPost]
        public async Task<ActionResult<AdminDto>> Create([FromForm] CreateAdminDto dto)
        {
            var createdAdmin = await _adminService.CreateAdminAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdAdmin.Id }, createdAdmin);
        }

        // PUT: api/admin/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<AdminDto>> Update(int id, [FromForm] UpdateAdminDto dto)
        {
            try
            {
                var updatedAdmin = await _adminService.UpdateAdminAsync(id, dto);
                if (updatedAdmin == null) return NotFound("Admin not found");
                return Ok(updatedAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/admin/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _adminService.DeleteAdminAsync(id);
            if (!result) return NotFound("Admin not found");
            return NoContent();
        }
    }
}
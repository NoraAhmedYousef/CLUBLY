using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignUp.DTO;
using SignUp.Service.Classes;
using SignUp.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace SignUp.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {

        private readonly IMemberService _service;

        public MembersController(IMemberService service)
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
            var member = await _service.GetByIdAsync(id);
            return member == null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMemberDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateMemberDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? Ok("Deleted") : NotFound();
        }
    }
}
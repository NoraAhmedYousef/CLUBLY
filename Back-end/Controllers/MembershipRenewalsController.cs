using Clubly.DTO;
using Clubly.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clubly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipRenewalsController : ControllerBase
    {
        private readonly IMembershipRenewalRequestService _service;
        public MembershipRenewalsController(IMembershipRenewalRequestService service)
            => _service = service;

        // GET: api/MembershipRenewals  → للأدمن (كل الطلبات)
        [HttpGet]
        public async Task<ActionResult<List<MembershipRenewalRequestDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        // GET: api/MembershipRenewals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipRenewalRequestDto>> GetById(int id)
        {
            var r = await _service.GetByIdAsync(id);
            return r is null ? NotFound() : Ok(r);
        }

        // GET: api/MembershipRenewals/by-member/3 → للعضو
        [HttpGet("by-member/{memberId}")]
        public async Task<ActionResult<List<MembershipRenewalRequestDto>>> GetByMember(int memberId)
            => Ok(await _service.GetByMemberAsync(memberId));

        // POST: api/MembershipRenewals  (multipart/form-data - زي FacilityBookings)
        [HttpPost]
        public async Task<ActionResult<MembershipRenewalRequestDto>> Create(
            [FromForm] CreateMembershipRenewalRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/MembershipRenewals/5/status → approve / reject
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateMembershipRenewalStatusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = await _service.UpdateStatusAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/MembershipRenewals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

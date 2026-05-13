using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Staff;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;
        public StaffController(IStaffRepository staffRepository)
            => _staffRepository = staffRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDetailsDTO>>> GetAll()
            => Ok(await _staffRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDetailsDTO>> GetById(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            return staff == null ? NotFound() : Ok(staff);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Add(StaffDTO dto)
        {
            var id = await _staffRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, StaffDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            await _staffRepository.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _staffRepository.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}

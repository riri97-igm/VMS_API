using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Staff;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;

        public StaffController(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaffs()
        {
            var staffs = await _staffRepository.GetAllAsync();
            return Ok(staffs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaffById(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddStaff(StaffDTO staffDto)
        {
            var staffId = await _staffRepository.AddAsync(staffDto);
            return CreatedAtAction(nameof(GetStaffById), new { id = staffId }, staffId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(int id, StaffDTO staffDto)
        {
            if (id != staffDto.Id)
            {
                return BadRequest();
            }

            var result = await _staffRepository.UpdateAsync(staffDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await _staffRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

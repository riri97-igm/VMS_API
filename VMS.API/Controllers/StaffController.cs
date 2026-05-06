//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using VMS.DataAccess;
//using VMS.DataAccess.Interface;
//using VMS.Model.DTOs.Staff;

//namespace VMS.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StaffController : ControllerBase
//    {
//        private readonly IStaffRepository _staffRepository;

//        public StaffController(IStaffRepository staffRepository)
//        {
//            _staffRepository = staffRepository;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaffs()
//        {
//            var staffs = await _staffRepository.GetAllAsync();
//            return Ok(staffs);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<StaffDTO>> GetStaffById(int id)
//        {
//            var staff = await _staffRepository.GetByIdAsync(id);
//            if (staff == null)
//            {
//                return NotFound();
//            }
//            return Ok(staff);
//        }

//        [HttpPost]
//        public async Task<ActionResult<int>> AddStaff(StaffDTO staffDto)
//        {
//            var staffId = await _staffRepository.AddAsync(staffDto);
//            return CreatedAtAction(nameof(GetStaffById), new { id = staffId }, staffId);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateStaff(int id, StaffDTO staffDto)
//        {
//            if (id != staffDto.Id)
//            {
//                return BadRequest();
//            }

//            var result = await _staffRepository.UpdateAsync(staffDto);
//            if (!result)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteStaff(int id)
//        {
//            var result = await _staffRepository.DeleteAsync(id);
//            if (!result)
//            {
//                return NotFound();
//            }

//            return NoContent();
//        }
//    }
//}


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

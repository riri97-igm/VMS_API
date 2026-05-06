//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using VMS.DataAccess;
//using VMS.Model.DTOs;

//namespace VMS.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DepartmentController : ControllerBase
//    {
//        private readonly IDepartmentRepository _departmentRepository;

//        public DepartmentController(IDepartmentRepository departmentRepository)
//        {
//            _departmentRepository = departmentRepository;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAllDepartments()
//        {
//            var departments = await _departmentRepository.GetAllAsync();
//            return Ok(departments);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int id)
//        {
//            var department = await _departmentRepository.GetByIdAsync(id);
//            if (department == null)
//            {
//                return NotFound();
//            }
//            return Ok(department);
//        }

//        [HttpPost]
//        public async Task<ActionResult<int>> AddDepartment(DepartmentDTO departmentDto)
//        {
//            var departmentId = await _departmentRepository.AddAsync(departmentDto);
//            return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentId }, departmentId);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateDepartment(int id,DepartmentDTO departmentDto)
//        {
//            if (id != departmentDto.Id)
//            {
//                return BadRequest();
//            }

//            await _departmentRepository.UpdateAsync(departmentDto);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteDepartment(int id, int changedBy)
//        {
//            await _departmentRepository.DeleteAsync(id, changedBy);
//            return NoContent();
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
            => _departmentRepository = departmentRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAll()
            => Ok(await _departmentRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetById(int id)
        {
            var dept = await _departmentRepository.GetByIdAsync(id);
            return dept == null ? NotFound() : Ok(dept);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Add(DepartmentDTO dto)
        {
            var id = await _departmentRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, DepartmentDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            await _departmentRepository.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _departmentRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
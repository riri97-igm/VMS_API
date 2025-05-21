using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess;
using VMS.Model.DTOs;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> GetAllDepartments()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDTO>> GetDepartmentById(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddDepartment(DepartmentDTO departmentDto)
        {
            var departmentId = await _departmentRepository.AddAsync(departmentDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentId }, departmentId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id,DepartmentDTO departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return BadRequest();
            }

            await _departmentRepository.UpdateAsync(departmentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id, int changedBy)
        {
            await _departmentRepository.DeleteAsync(id, changedBy);
            return NoContent();
        }
    }
}

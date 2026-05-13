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
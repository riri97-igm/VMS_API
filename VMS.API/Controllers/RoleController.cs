using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Role;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
            => _roleRepository = roleRepository;

        [HttpGet]
        [Authorize] // all logged-in users can read roles (needed for dropdowns)
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAll()
            => Ok(await _roleRepository.GetAllAsync());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(RoleDTO dto)
        {
            var id = await _roleRepository.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            var result = await _roleRepository.UpdateAsync(dto);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleRepository.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}

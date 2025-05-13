using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Role;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }
        [HttpPost]
        public async Task<ActionResult<int>> AddRole(RoleDTO roleDto)
        {
            var roleId = await _roleRepository.AddAsync(roleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = roleId }, roleId);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, RoleDTO roleDto)
        {
            if (id != roleDto.Id)
                return BadRequest("Role ID mismatch");
            var result = await _roleRepository.UpdateAsync(roleDto);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var result = await _roleRepository.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Visitor;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : Controller
    {
        private readonly IVisitorRepository _visitorRepository;

        public VisitorController(IVisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorDTO>>> GetAllVisitors()
        {
            var visitors = await _visitorRepository.GetAllAsync();
            return Ok(visitors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorDTO>> GetVisitorById(int id)
        {
            var visitor = await _visitorRepository.GetByIdAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }
            return Ok(visitor);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddVisitor(VisitorDTO visitorDto)
        {
            var visitorId = await _visitorRepository.AddAsync(visitorDto);
            return CreatedAtAction(nameof(GetVisitorById), new { id = visitorId }, visitorId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisitor(int id, VisitorDTO visitorDto)
        {
            if (id != visitorDto.Id)
            {
                return BadRequest();
            }

            await _visitorRepository.UpdateAsync(visitorDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            await _visitorRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

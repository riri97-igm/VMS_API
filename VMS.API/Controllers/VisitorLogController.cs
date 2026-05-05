using Microsoft.AspNetCore.Mvc;
using VMS.API.Services;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.VisitorLog;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorLogController : ControllerBase
    {
        private readonly IVisitorLogRepository _logRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IEmailService _emailService;

        public VisitorLogController(
            IVisitorLogRepository logRepository,
            IStaffRepository staffRepository,
            IVisitorRepository visitorRepository,
            IEmailService emailService)
        {
            _logRepository = logRepository;
            _staffRepository = staffRepository;
            _visitorRepository = visitorRepository;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorLogDetailsDTO>>> GetAll()
        {
            var logs = await _logRepository.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<VisitorLogDetailsDTO>>> GetToday()
        {
            var logs = await _logRepository.GetTodayLogsAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorLogDetailsDTO>> GetById(int id)
        {
            var log = await _logRepository.GetByIdAsync(id);
            return log == null ? NotFound() : Ok(log);
        }

        [HttpGet("visitor/{visitorId}")]
        public async Task<ActionResult<IEnumerable<VisitorLogDetailsDTO>>> GetByVisitor(int visitorId)
        {
            var logs = await _logRepository.GetByVisitorIdAsync(visitorId);
            return Ok(logs);
        }

        [HttpPost("checkin")]
        public async Task<ActionResult<int>> CheckIn(VisitorLogDTO dto)
        {
            var logId = await _logRepository.CheckInAsync(dto);

            // Send email notification to host staff
            try
            {
                var staff = await _staffRepository.GetByIdAsync(dto.StaffId);
                var visitor = await _visitorRepository.GetByIdAsync(dto.VisitorId);
                if (staff != null && visitor != null)
                {
                    await _emailService.SendVisitorCheckInNotificationAsync(
                        staff.Email,
                        staff.Name,
                        $"{visitor.FirstName} {visitor.LastName}",
                        visitor.Company ?? "",
                        DateTime.UtcNow,
                        "Walk-in visit");
                }
            }
            catch { /* email failure should not break check-in */ }

            return CreatedAtAction(nameof(GetById), new { id = logId }, logId);
        }

        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> CheckOut(int id, [FromBody] CheckOutRequest req)
        {
            var success = await _logRepository.CheckOutAsync(id, req.Remarks);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _logRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public record CheckOutRequest(string? Remarks);
}

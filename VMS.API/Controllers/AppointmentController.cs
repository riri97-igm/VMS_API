<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
=======
﻿//using Microsoft.AspNetCore.Mvc;
//using VMS.DataAccess.Interface;
//using VMS.Model.DTOs.Appointment;

//namespace VMS.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AppointmentController : Controller
//    {
//        private readonly IAppointmentRepository _appointmentRepository;

//        public AppointmentController(IAppointmentRepository appointmentRepository)
//        {
//            _appointmentRepository = appointmentRepository;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointments()
//        {
//            var appointments = await _appointmentRepository.GetAllAsync();
//            return Ok(appointments);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<AppointmentDTO>> GetAppointmentById(int id)
//        {
//            var appointment = await _appointmentRepository.GetByIdAsync(id);
//            if (appointment == null)
//            {
//                return NotFound();
//            }
//            return Ok(appointment);
//        }

//        [HttpPost]
//        public async Task<ActionResult<int>> AddAppointment(AppointmentDTO appointmentDto)
//        {
//            var appointmentId = await _appointmentRepository.AddAsync(appointmentDto);
//            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, appointmentId);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDTO appointmentDto)
//        {
//            if (id != appointmentDto.Id)
//            {
//                return BadRequest();
//            }

//            await _appointmentRepository.UpdateAsync(appointmentDto);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAppointment(int id)
//        {
//            await _appointmentRepository.DeleteAsync(id);
//            return NoContent();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
>>>>>>> 31e87524baa9da70bb20cab8e78177a907295f30
using VMS.API.Services;
using VMS.Common.Enums;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Appointment;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IVisitorRepository _visitorRepository;
        private readonly IEmailService _emailService;

        public AppointmentController(
            IAppointmentRepository appointmentRepository,
            IStaffRepository staffRepository,
            IVisitorRepository visitorRepository,
            IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _staffRepository = staffRepository;
            _visitorRepository = visitorRepository;
            _emailService = emailService;
        }

        [HttpGet]
<<<<<<< HEAD
        public async Task<ActionResult<IEnumerable<AppointmentDetailsDTO>>> GetAllAppointments() // changed
=======
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAllAppointments()
>>>>>>> 31e87524baa9da70bb20cab8e78177a907295f30
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailsDTO>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            return appointment == null ? NotFound() : Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddAppointment(AppointmentDTO appointmentDto)
        {
            var appointmentId = await _appointmentRepository.AddAsync(appointmentDto);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointmentId }, appointmentId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentDTO appointmentDto)
        {
            if (id != appointmentDto.Id) return BadRequest();

<<<<<<< HEAD
            var existing = await _appointmentRepository.GetByIdAsync(id);
            await _appointmentRepository.UpdateAsync(appointmentDto);

=======
            // Load existing to detect status change
            var existing = await _appointmentRepository.GetByIdAsync(id);
            await _appointmentRepository.UpdateAsync(appointmentDto);

            // Send email if status changed to Approved or Rejected
>>>>>>> 31e87524baa9da70bb20cab8e78177a907295f30
            if (existing != null && existing.Status != appointmentDto.Status)
            {
                try
                {
                    var visitor = await _visitorRepository.GetByIdAsync(appointmentDto.VisitorId);
                    var staff = await _staffRepository.GetByIdAsync(appointmentDto.StaffId);
                    if (visitor != null && staff != null)
                    {
                        var visitorName = $"{visitor.FirstName} {visitor.LastName}";
                        if (appointmentDto.Status == AppointmentStatus.Approved)
                            await _emailService.SendAppointmentApprovedAsync(
                                visitor.Email, visitorName, staff.Name,
                                appointmentDto.AppointmentDate, appointmentDto.Purpose);
                        else if (appointmentDto.Status == AppointmentStatus.Rejected)
                            await _emailService.SendAppointmentRejectedAsync(
                                visitor.Email, visitorName, staff.Name,
                                appointmentDto.AppointmentDate, "Please contact reception for details.");
                    }
                }
                catch { /* email failure should not break update */ }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentRepository.DeleteAsync(id);
            return NoContent();
        }

<<<<<<< HEAD
=======
        // Dashboard stats endpoint
>>>>>>> 31e87524baa9da70bb20cab8e78177a907295f30
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var all = (await _appointmentRepository.GetAllAsync()).ToList();
            return Ok(new
            {
                Total = all.Count,
                Pending = all.Count(a => a.Status == AppointmentStatus.Pending),
                Approved = all.Count(a => a.Status == AppointmentStatus.Approved),
                Completed = all.Count(a => a.Status == AppointmentStatus.Completed),
                Rejected = all.Count(a => a.Status == AppointmentStatus.Rejected),
            });
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 31e87524baa9da70bb20cab8e78177a907295f30

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VMS.DataAccess.Interface;

namespace VMS.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ReportController : ControllerBase
	{
		private readonly IVisitorLogRepository _logRepo;
		private readonly IAppointmentRepository _appointRepo;
		private readonly IVisitorRepository _visitorRepo;
		private readonly IStaffRepository _staffRepo;

		public ReportController(
			IVisitorLogRepository logRepo,
			IAppointmentRepository appointRepo,
			IVisitorRepository visitorRepo,
			IStaffRepository staffRepo)
		{
			_logRepo = logRepo;
			_appointRepo = appointRepo;
			_visitorRepo = visitorRepo;
			_staffRepo = staffRepo;
		}

		[HttpGet("summary")]
		public async Task<ActionResult> GetSummary([FromQuery] string? from, [FromQuery] string? to)
		{
			// Parse as date-only, default to last 30 days
			var fromDate = string.IsNullOrEmpty(from)
				? DateTime.UtcNow.AddDays(-30).Date
				: DateTime.Parse(from).Date;

			var toDate = string.IsNullOrEmpty(to)
				? DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1) // end of today
				: DateTime.Parse(to).Date.AddDays(1).AddSeconds(-1); // end of selected day

			var allLogs = (await _logRepo.GetAllAsync()).ToList();
			var allAppts = (await _appointRepo.GetAllAsync()).ToList();
			var allVisitors = (await _visitorRepo.GetAllAsync()).ToList();

			// Filter by date range — compare date part only to avoid timezone issues
			var logs = allLogs.Where(l => l.CheckInTime.Date >= fromDate && l.CheckInTime.Date <= toDate.Date).ToList();
			var appts = allAppts.Where(a => a.AppointmentDate.Date >= fromDate && a.AppointmentDate.Date <= toDate.Date).ToList();

			// Visitors per day
			var visitorsPerDay = logs
				.GroupBy(l => l.CheckInTime.Date)
				.OrderBy(g => g.Key)
				.Select(g => new { date = g.Key.ToString("yyyy-MM-dd"), count = g.Count() })
				.ToList();

			// Appointments by status
			var apptByStatus = appts
				.GroupBy(a => a.Status.ToString())
				.Select(g => new { status = g.Key, count = g.Count() })
				.ToList();

			// Visits by department
			var visitsByDept = logs
				.GroupBy(l => l.Staff?.DepartmentName ?? "Unknown")
				.Select(g => new { department = g.Key, count = g.Count() })
				.OrderByDescending(g => g.count)
				.ToList();

			// Top 5 most visited staff
			var topStaff = logs
				.GroupBy(l => l.Staff?.Name ?? "Unknown")
				.Select(g => new { staff = g.Key, count = g.Count() })
				.OrderByDescending(g => g.count)
				.Take(5)
				.ToList();

			// Walk-in vs appointment
			var walkIns = logs.Count(l => l.AppointmentId == null);
			var withAppoint = logs.Count(l => l.AppointmentId != null);

			// Average visit duration in minutes
			var completed = logs.Where(l => l.CheckOutTime.HasValue).ToList();
			var avgMinutes = completed.Any()
				? completed.Average(l => (l.CheckOutTime!.Value - l.CheckInTime).TotalMinutes)
				: 0;

			return Ok(new
			{
				Period = new { From = fromDate, To = toDate },
				TotalVisits = logs.Count,
				TotalAppointments = appts.Count,
				TotalVisitors = allVisitors.Count,
				WalkIns = walkIns,
				WithAppointment = withAppoint,
				AvgVisitMinutes = Math.Round(avgMinutes, 1),
				CurrentlyInside = allLogs.Count(l => !l.CheckOutTime.HasValue),
				VisitorsPerDay = visitorsPerDay,
				AppointmentsByStatus = apptByStatus,
				VisitsByDepartment = visitsByDept,
				TopStaff = topStaff,
			});
		}
	}
}
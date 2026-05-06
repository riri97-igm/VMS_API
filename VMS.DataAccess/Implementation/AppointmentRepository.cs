using Microsoft.EntityFrameworkCore;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.Appointment;

namespace VMS.DataAccess.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DbVMSDataContext _context;

        public AppointmentRepository(DbVMSDataContext context)
        {
            _context = context;
        }

        // Changed return type to IEnumerable<AppointmentDetailsDTO>
        public async Task<IEnumerable<AppointmentDetailsDTO>> GetAllAsync()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Visitor)
                .Include(a => a.Staff)
                    .ThenInclude(s => s.Department)
                .ToListAsync();

            return appointments.Select(AppointmentConverter.ToAppointmentDetailsDTO);
        }

        public async Task<AppointmentDetailsDTO?> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Visitor)
                .Include(a => a.Staff)
                    .ThenInclude(s => s.Department)
                .FirstOrDefaultAsync(a => a.Id == id);

            return appointment == null ? null : AppointmentConverter.ToAppointmentDetailsDTO(appointment);
        }

        public async Task<int> AddAsync(AppointmentDTO appointmentDto)
        {
            var appointment = AppointmentConverter.ToAppointmentEntity(appointmentDto);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment.Id;
        }

        public async Task UpdateAsync(AppointmentDTO appointmentDto)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointmentDto.Id);
            if (existingAppointment == null) return;

            existingAppointment.VisitorId = appointmentDto.VisitorId;
            existingAppointment.StaffId = appointmentDto.StaffId;
            existingAppointment.Purpose = appointmentDto.Purpose;
            existingAppointment.AppointmentDate = appointmentDto.AppointmentDate;
            existingAppointment.Status = appointmentDto.Status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return;
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.Appointment;

namespace VMS.DataAccess.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DbVMSDataContext _context;
        private readonly IVisitorRepository _visitorRepository;

        public AppointmentRepository(DbVMSDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAsync()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return appointments.Select(AppointmentConverter.ToAppointmentDTO);
        }

        public async Task<AppointmentDetailsDTO?> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Visitor)
                .Include(a => a.Staff)
                .FirstOrDefaultAsync(a => a.Id == id);

            return appointment != null ? null : AppointmentConverter.ToAppointmentDetailsDTO(appointment) ;
        }

        public async Task<int> AddAsync(AppointmentDTO appointmentDto)
        {
            var appointment = AppointmentConverter.ToAppointmentEntity(appointmentDto);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            //await _auditLogRepository.AddAsync(new AuditLogDTO
            //{
            //    TableName = "Appointment",
            //    RecordId = appointment.Id,
            //    Action = AuditAction.Insert.ToString(),
            //    NewValues = JsonSerializer.Serialize(appointment),
            //    ChangedBy = appointmentDto.ChangedBy,
            //    ChangedAt = DateTime.UtcNow
            //});

            return appointment.Id;
        }

        public async Task UpdateAsync(AppointmentDTO appointmentDto)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointmentDto.Id);
            if (existingAppointment == null) return;

            var oldValues = JsonSerializer.Serialize(existingAppointment);
            existingAppointment.VisitorId = appointmentDto.VisitorId;
            existingAppointment.StaffId = appointmentDto.StaffId;
            existingAppointment.Purpose = appointmentDto.Purpose;
            existingAppointment.AppointmentDate = appointmentDto.AppointmentDate;
            existingAppointment.Status = appointmentDto.Status;
            await _context.SaveChangesAsync();

            //await _auditLogRepository.AddAsync(new AuditLogDTO
            //{
            //    TableName = "Appointment",
            //    RecordId = existingAppointment.Id,
            //    Action = AuditAction.Update.ToString(),
            //    OldValues = oldValues,
            //    NewValues = JsonSerializer.Serialize(existingAppointment),
            //    ChangedBy = appointmentDto.ChangedBy,
            //    ChangedAt = DateTime.UtcNow
            //});
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            //await _auditLogRepository.AddAsync(new AuditLogDTO
            //{
            //    TableName = "Appointment",
            //    RecordId = id,
            //    Action = AuditAction.Delete.ToString(),
            //    OldValues = JsonSerializer.Serialize(appointment),
            //    //ChangedBy = changedBy,
            //    ChangedAt = DateTime.UtcNow
            //});
        }
    }
}


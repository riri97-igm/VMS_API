using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.Appointment;

namespace VMS.Converter
{
    public static class AppointmentConverter
    {
        public static AppointmentDTO ToAppointmentDTO (Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id,
                VisitorId = appointment.VisitorId,
                StaffId = appointment.StaffId,
                Purpose = appointment.Purpose,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
            };
        }
        public static Appointment ToAppointmentEntity (AppointmentDTO appointmentDto)
        {
            return new Appointment
            {
                Id = appointmentDto.Id,
                VisitorId = appointmentDto.VisitorId,
                StaffId = appointmentDto.StaffId,
                Purpose = appointmentDto.Purpose,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status,
            };
        }

        public static AppointmentDetailsDTO ToAppointmentDetailsDTO (Appointment appointment)
        {
            return new AppointmentDetailsDTO
            {
                Id = appointment.Id,
                Visitor = VisitorConverter.ToVisitorDTO(appointment.Visitor),
                Staff = StaffConverter.ToStaffDTO(appointment.Staff),
                Purpose = appointment.Purpose,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
            };
        }
    }
}

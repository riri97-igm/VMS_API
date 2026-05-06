using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.VisitorLog;

namespace VMS.Converter
{
    public static class VisitorLogConverter
    {
        public static VisitorLogDTO ToVisitorLogDTO(VisitorLog log)
        {
            return new VisitorLogDTO
            {
                Id = log.Id,
                VisitorId = log.VisitorId,
                StaffId = log.StaffId,
                AppointmentId = log.AppointmentId ?? 0,
                CheckInTime = log.CheckInTime,
                CheckOutTime = log.CheckOutTime,
                Remarks = log.Remarks,
                ChangedBy = log.ChangedBy,
            };
        }

        public static VisitorLogDetailsDTO ToVisitorLogDetailsDTO(VisitorLog log)
        {
            return new VisitorLogDetailsDTO
            {
                Id = log.Id,
                VisitorId = log.VisitorId,
                StaffId = log.StaffId,
                AppointmentId = log.AppointmentId ?? 0,
                CheckInTime = log.CheckInTime,
                CheckOutTime = log.CheckOutTime,
                Remarks = log.Remarks,
                ChangedBy = log.ChangedBy,
                Visitor = log.Visitor != null ? VisitorConverter.ToVisitorDTO(log.Visitor) : new(),
                Staff = log.Staff != null ? StaffConverter.ToStaffDTO(log.Staff) : new(),
                Appointment = log.Appointment != null ? AppointmentConverter.ToAppointmentDTO(log.Appointment) : null,
            };
        }

        public static VisitorLog ToVisitorLogEntity(VisitorLogDTO dto)
        {
            return new VisitorLog
            {
                Id = dto.Id,
                VisitorId = dto.VisitorId,
                StaffId = dto.StaffId,
                AppointmentId = dto.AppointmentId == 0 ? null : dto.AppointmentId,
                CheckInTime = dto.CheckInTime,
                CheckOutTime = dto.CheckOutTime,
                Remarks = dto.Remarks,
                ChangedBy = dto.ChangedBy,
            };
        }
    }
}

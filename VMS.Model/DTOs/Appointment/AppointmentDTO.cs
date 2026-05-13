using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Common.Enums;

namespace VMS.Model.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int StaffId { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public int ChangeBy { get; set; }
    }
}

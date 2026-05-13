using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs.Appointment;
using VMS.Model.DTOs.Staff;
using VMS.Model.DTOs.Visitor;

namespace VMS.Model.DTOs.VisitorLog
{
    public class VisitorLogDetailsDTO : VisitorLogDTO
    {
        public VisitorDTO Visitor { get; set; } = new VisitorDTO();
        public StaffDTO Staff { get; set; } = new StaffDTO();
        public AppointmentDTO? Appointment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Common.Enums;
using VMS.Model.DTOs.Staff;
using VMS.Model.DTOs.Visitor;

namespace VMS.Model.DTOs.Appointment
{
    public class AppointmentDetailsDTO
    {
        public int Id { get; set; }
        public VisitorDTO Visitor { get; set; } = new VisitorDTO();
        public StaffDTO Staff { get; set; } = new StaffDTO();
        public DateTime AppointmentDate { get; set; }
        public string Purpose { get; set; } = string.Empty; 
        public AppointmentStatus Status { get; set; }

    }
}

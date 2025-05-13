using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Common.Enums;

namespace VMS.EntityFramework.EntityModel
{
    public class Appointment
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; } 
        public Staff Staff { get; set; } 
        public int StaffId { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}

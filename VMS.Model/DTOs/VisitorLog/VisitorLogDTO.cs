using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Model.DTOs.VisitorLog
{
    public class VisitorLogDTO
    {
        public int Id { get; set; }
        public int VisitorId { get; set; }
        public int StaffId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Remarks { get; set; }
        public int ChangedBy { get; set; }  
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Model.DTOs.Staff
{
    public class StaffDetailsDTO : StaffDTO
    {
        public string DepartmentName { get; set; } = string.Empty;
    }
}

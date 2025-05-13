using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.Staff;

namespace VMS.Converter
{
    public static class StaffConverter
    {
        public static StaffDTO ToStaffDTO(Staff staff)
        {
            return new StaffDTO
            {
                Id = staff.Id,
                Name = staff.Name,
                Email = staff.Email,
                Phone = staff.Phone,
                DepartmetId = staff.DepartmentId,
                RoleId = staff.RoleId
            };
        }

        public static Staff ToStaffEntity(StaffDTO staffDto)
        {
            return new Staff
            {
                Id = staffDto.Id,
                Name = staffDto.Name,
                Email = staffDto.Email,
                Phone = staffDto.Phone,
                DepartmentId = staffDto.DepartmetId,
                RoleId = staffDto.RoleId
            };
        }

        public static StaffDetailsDTO ToStaffDetailDTO(Staff staff)
        {
            return new StaffDetailsDTO()
            {
                Id = staff.Id,
                Name = staff.Name,
                Email = staff.Email,
                Phone = staff.Phone,
                DepartmetId = staff.DepartmentId,
                DepartmentName = staff.Department?.Name ?? string.Empty,
                RoleName = staff.Role?.Name ?? string.Empty,
                RoleId = staff.RoleId

            };
        }
    }
}

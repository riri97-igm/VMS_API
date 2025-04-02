using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs;

namespace VMS.Converter
{
    public static class DepartmentConverter
    {
        public static DepartmentDTO ToDepartmentDTO(Department department)
        {
            return new DepartmentDTO
            {
                Id = department.Id,
                Name = department.Name,
            };
        }

        public static Department ToDepartmentEntity (DepartmentDTO departmentDto) 
        {
            return new Department
            {
                Id = departmentDto.Id,
                Name = departmentDto.Name,
            };
        }
    }
}

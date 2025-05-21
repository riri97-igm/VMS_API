using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Converter;
using VMS.EntityFramework;
using VMS.Model.DTOs;

namespace VMS.DataAccess
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DbVMSDataContext _context;
        
        public DepartmentRepository(DbVMSDataContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var departments = await _context.Departments.ToListAsync();
            var result = new List<DepartmentDTO>();
            
            foreach (var department in departments)
            {
                var dto = DepartmentConverter.ToDepartmentDTO(department);
                
                if (department.ChangedBy > 0)
                {
                    var staff = await _context.Staffs.FindAsync(department.ChangedBy);
                    if (staff != null)
                    {
                        dto.ChangedByName = staff.Name;
                    }
                }
                
                result.Add(dto);
            }
            
            return result;
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return null;
            
            var dto = DepartmentConverter.ToDepartmentDTO(department);
            
            if (department.ChangedBy > 0)
            {
                var staff = await _context.Staffs.FindAsync(department.ChangedBy);
                if (staff != null)
                {
                    dto.ChangedByName = staff.Name;
                }
            }
            
            return dto;
        }

        public async Task<int> AddAsync(DepartmentDTO departmentDto)
        {
            var department = DepartmentConverter.ToDepartmentEntity(departmentDto);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department.Id;
        }

        public async Task UpdateAsync(DepartmentDTO departmentDto)
        {
            var department = await _context.Departments.FindAsync(departmentDto.Id);
            if (department == null) return;
            
            department.Name = departmentDto.Name;
            department.ChangedBy = departmentDto.ChangedBy;
            
            await _context.SaveChangesAsync();
            
            // Update the ChangedByName in the DTO for the response
            if (departmentDto.ChangedBy > 0)
            {
                var staff = await _context.Staffs.FindAsync(departmentDto.ChangedBy);
                if (staff != null)
                {
                    departmentDto.ChangedByName = staff.Name;
                }
            }
        }

        public async Task DeleteAsync(int id, int changedBy)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return;
            
            // Instead of removing the department, we could update its ChangedBy field
            // before deletion if that's part of your audit trail requirements
            department.ChangedBy = changedBy;
            
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}

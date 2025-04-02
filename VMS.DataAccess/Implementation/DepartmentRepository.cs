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
            return departments.Select(DepartmentConverter.ToDepartmentDTO);
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department == null ? null : DepartmentConverter.ToDepartmentDTO(department);
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
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, int changedBy)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return;
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}

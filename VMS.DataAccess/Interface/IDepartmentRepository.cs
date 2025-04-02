using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs;

namespace VMS.DataAccess
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<DepartmentDTO?> GetByIdAsync(int id);
        Task<int> AddAsync (DepartmentDTO departmentDto);
        Task UpdateAsync (DepartmentDTO departmentDto);
        Task DeleteAsync (int id, int changedby);
    }
}

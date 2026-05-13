using VMS.Model.DTOs;

namespace VMS.DataAccess
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<DepartmentDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(DepartmentDTO departmentDto);
        Task UpdateAsync(DepartmentDTO departmentDto);
        Task DeleteAsync(int id);
    }
}


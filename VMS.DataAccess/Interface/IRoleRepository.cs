using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs.Role;

namespace VMS.DataAccess.Interface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDTO>> GetAllAsync();
        Task<RoleDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(RoleDTO roleDto);
        Task<bool> UpdateAsync (RoleDTO roleDto);
        Task<bool> DeleteAsync(int id);
    }
}

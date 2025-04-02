using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs.Staff;

namespace VMS.DataAccess.Interface
{
    public interface IStaffRepository
    {
        Task<int> AddAsync(StaffDTO staffDto);
        Task<bool> UpdateAsync(StaffDTO staffDto);
        Task <bool> DeleteAsync(int id);
        Task <IEnumerable<StaffDetailsDTO>> GetAllAsync();
        Task <StaffDetailsDTO> GetByIdAsync(int id);
    }
}

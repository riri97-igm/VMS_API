using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Model.DTOs.Visitor;

namespace VMS.DataAccess.Interface
{
    public interface IVisitorRepository
    {
        Task<IEnumerable<VisitorDTO>> GetAllAsync();
        Task<VisitorDTO?> GetByIdAsync(int id);
        Task<int> AddAsync(VisitorDTO visitorDto);
        Task UpdateAsync(VisitorDTO visitorDto);
        Task DeleteAsync(int id);

    }
}

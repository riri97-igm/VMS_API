using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Staff;
using System.Text.Json;
using VMS.Converter;
using Microsoft.EntityFrameworkCore;

namespace VMS.DataAccess.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DbVMSDataContext _context;

        public StaffRepository(DbVMSDataContext context)
        {
            _context = context;
           
        }
        public async Task<IEnumerable<StaffDetailsDTO>> GetAllAsync()
        {
            try
            {
                var staffs = await _context.Staffs
                    .Include(s => s.Department)
                    .Include(s => s.Role)
                    .ToListAsync();
                return staffs.Select(StaffConverter.ToStaffDetailDTO);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return null;
        }

        public async Task<StaffDetailsDTO?> GetByIdAsync(int id)
        {
            var staff = await _context.Staffs
                .Include(s => s.Department)
                .Include(s => s.Role)
                .FirstOrDefaultAsync(s => s.Id == id);

            return staff != null ? StaffConverter.ToStaffDetailDTO(staff) : null;
        }
        public async Task<int> AddAsync(StaffDTO staffDto)
        {
            var staff = StaffConverter.ToStaffEntity(staffDto);
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return staff.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return false;

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(StaffDTO staffDto)
        {
            var staff = await _context.Staffs.FindAsync(staffDto.Id);
            if (staff == null) return false;

            var oldValues = JsonSerializer.Serialize(staff);

            staff.Name = staffDto.Name;
            staff.Email = staffDto.Email;
            staff.Phone = staffDto.Phone;
            staff.DepartmentId = staffDto.DepartmetId;
            staff.RoleId = staffDto.RoleId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

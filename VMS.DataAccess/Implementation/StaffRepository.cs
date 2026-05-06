using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.Staff;

namespace VMS.DataAccess.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DbVMSDataContext _context;

        public StaffRepository(DbVMSDataContext context)
            => _context = context;

        public async Task<IEnumerable<StaffDetailsDTO>> GetAllAsync()
        {
            var staffs = await _context.Staffs
                .Include(s => s.Department)
                .Include(s => s.Role)
                .ToListAsync();
            return staffs.Select(StaffConverter.ToStaffDetailDTO);
        }

        public async Task<StaffDetailsDTO> GetByIdAsync(int id)
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

            // Changed from HMACSHA512 to HMACSHA256
            // Default password = "Staff@123"
            using var hmac = new HMACSHA256();
            staff.PasswordSalt = hmac.Key;
            staff.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Staff@123"));

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
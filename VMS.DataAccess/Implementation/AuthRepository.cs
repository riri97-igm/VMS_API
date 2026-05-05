//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VMS.DataAccess.Interface;
//using VMS.EntityFramework;
//using VMS.EntityFramework.EntityModel;

//namespace VMS.DataAccess.Implementation
//{
//    public class AuthRepository : ILoginRepository
//    {
//        private readonly DbVMSDataContext _context;
//        public AuthRepository(DbVMSDataContext context)
//        {
//            _context = context;
//        }

//        public async Task<Login> GetByUserNameAndPasswordAsync(string userName, string password)
//        {
//            return await _context.Logins.FirstOrDefaultAsync(l => l.UserName == userName && l.Password == password);
//        }
//    }
//}

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.Auth;

namespace VMS.DataAccess.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DbVMSDataContext _context;

        public AuthRepository(DbVMSDataContext context)
        {
            _context = context;
        }

        // --- Password helpers ---
        private static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            return (hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
        }

        private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }

        public async Task<(bool Success, string RoleName, Staff? Staff)> LoginAsync(LoginRequestDTO dto)
        {
            var staff = await _context.Staffs
                .Include(s => s.Role)
                .FirstOrDefaultAsync(s => s.Email.ToLower() == dto.Email.ToLower());

            if (staff == null) return (false, "", null);
            if (staff.PasswordHash.Length == 0) return (false, "", null); // account not set up
            if (!VerifyPassword(dto.Password, staff.PasswordHash, staff.PasswordSalt))
                return (false, "", null);

            return (true, staff.Role?.Name ?? "Staff", staff);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDTO dto)
        {
            if (await _context.Staffs.AnyAsync(s => s.Email.ToLower() == dto.Email.ToLower()))
                return (false, "Email already exists");

            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId);
            if (!roleExists) return (false, "Invalid role");

            var deptExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!deptExists) return (false, "Invalid department");

            var (hash, salt) = HashPassword(dto.Password);

            var staff = new Staff
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                DepartmentId = dto.DepartmentId,
                RoleId = dto.RoleId,
                PasswordHash = hash,
                PasswordSalt = salt,
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return (true, "Registered successfully");
        }

        public async Task<bool> ChangePasswordAsync(int staffId, string currentPassword, string newPassword)
        {
            var staff = await _context.Staffs.FindAsync(staffId);
            if (staff == null) return false;
            if (!VerifyPassword(currentPassword, staff.PasswordHash, staff.PasswordSalt)) return false;

            var (hash, salt) = HashPassword(newPassword);
            staff.PasswordHash = hash;
            staff.PasswordSalt = salt;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

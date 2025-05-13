using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Converter;
using VMS.DataAccess.Interface;
using VMS.EntityFramework;
using VMS.Model.DTOs.Role;

namespace VMS.DataAccess.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DbVMSDataContext _context;

        public RoleRepository(DbVMSDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles.Select(RoleConverter.toRoleDTO);
                
        }
        public async Task<RoleDTO?> GetByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role != null ? null : RoleConverter.toRoleDTO(role);
        }
        public async Task<int> AddAsync(RoleDTO roleDto)
        {
            var role = RoleConverter.toRoleEntity(roleDto);
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role.Id;
        }

        public async Task<bool> UpdateAsync(RoleDTO roleDto)
        {
            var role = await _context.Roles.FindAsync(roleDto.Id);
            if (role == null)
                return false;
            
            role.Name = roleDto.Name;
            role.Description = roleDto.Description;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

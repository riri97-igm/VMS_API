using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;
using VMS.Model.DTOs.Role;

namespace VMS.Converter
{
    public static class RoleConverter
    {
        public static RoleDTO toRoleDTO(Role role)
        {
            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public static Role toRoleEntity(RoleDTO roleDTO)
        {
            return new Role
            {
                Id = roleDTO.Id,
                Name = roleDTO.Name,
                Description = roleDTO.Description
            };
        }
    }
}

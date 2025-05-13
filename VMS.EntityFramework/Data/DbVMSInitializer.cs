using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;

namespace VMS.EntityFramework.Data
{
    public  class DbVMSInitializer
    {
        public static void Initialize(DbVMSDataContext context)

        {
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Name = "Human Resources" },
                    new Department { Name = "IT Department" },
                    new Department { Name = "Finance" }
                    );
                context.SaveChanges();
            }


            //Seed Roles
            if (!context.Roles.Any())
            {
                Role role =
                    new Role
                    {
                        Name = "Admin",
                        Description = "Administrator role"
                    };
                context.Roles.Add(role);

                context.SaveChanges();
                if(role.Id != 0 && role.Name == "Admin")
                {
                    // Seed Staff
                    if (!context.Staffs.Any())
                    {
                        var hrDept = context.Departments.First(d => d.Name == "Human Resources");
                        var itDept = context.Departments.First(d => d.Name == "IT Department");

                        context.Staffs.AddRange(
                            new Staff
                            {
                                Name = "Sarah",
                                Email = "sarah@company.com",
                                Phone = "123-456-7890",
                                DepartmentId = hrDept.Id,
                                RoleId = role.Id
                            },
                            new Staff
                            {
                                Name = "Alice",
                                Email = "alice.smith@company.com",
                                Phone = "987-654-3210",
                                DepartmentId = itDept.Id,
                                RoleId = role.Id
                            }
                        );
                        context.SaveChanges();
                    }
                }
            }

           
        }
    }
}

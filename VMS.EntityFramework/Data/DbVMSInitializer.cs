//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using VMS.EntityFramework.EntityModel;

//namespace VMS.EntityFramework.Data
//{
//    public  class DbVMSInitializer
//    {
//        public static void Initialize(DbVMSDataContext context)

//        {
//            if (!context.Departments.Any())
//            {
//                context.Departments.AddRange(
//                    new Department { Name = "Human Resources" },
//                    new Department { Name = "IT Department" },
//                    new Department { Name = "Finance" }
//                    );
//                context.SaveChanges();
//            }


//            //Seed Roles
//            if (!context.Roles.Any())
//            {
//                Role role =
//                    new Role
//                    {
//                        Name = "Admin",
//                        Description = "Administrator role"
//                    };
//                context.Roles.Add(role);

//                context.SaveChanges();
//                if (role.Id != 0 && role.Name == "Admin")
//                {
//                    // Seed Staff
//                    if (!context.Staffs.Any())
//                    {
//                        var hrDept = context.Departments.First(d => d.Name == "Human Resources");
//                        var itDept = context.Departments.First(d => d.Name == "IT Department");


//                        context.Staffs.AddRange(
//                            new Staff
//                            {
//                                Name = "Sarah",
//                                Email = "sarah@company.com",
//                                Phone = "123-456-7890",
//                                DepartmentId = hrDept.Id,
//                                RoleId = role.Id
//                            },
//                            new Staff
//                            {
//                                Name = "Alice",
//                                Email = "alice.smith@company.com",
//                                Phone = "987-654-3210",
//                                DepartmentId = itDept.Id,
//                                RoleId = role.Id
//                            }
//                        );
//                        context.SaveChanges();
//                    }
//                }
//            }

//            // Seed Visitors
//            if (!context.Visitors.Any())
//            {
//                context.Visitors.AddRange(
//                    new Visitor
//                    {
//                        FirstName = "Michael",
//                        LastName = "Johnson",
//                        Email = "michael.johnson@example.com",
//                        Phone = "555-123-4567",
//                        Company = "ABC Corp",
//                        IdentificationNumber = "ID12345"
//                    },
//                    new Visitor
//                    {
//                        FirstName = "Emma",
//                        LastName = "Brown",
//                        Email = "emma.brown@example.com",
//                        Phone = "555-987-6543",
//                        Company = "XYZ Ltd",
//                        IdentificationNumber = "ID67890"
//                    }
//                );
//                context.SaveChanges();
//            }

//            //Seed Logins
//            if (context.Logins.Any())
//            {
//                return; // DB has been seeded
//            }

//            var logins = new Login[]
//            {
//                new Login { UserName = "admin", Password = "admin123"},
//                new Login { UserName = "user", Password = "user123" }
//            };

//            foreach (var login in logins)
//            {
//                context.Logins.Add(login);
//            }

//            context.SaveChanges();
//        }
//    }


//}


using System.Security.Cryptography;
using System.Text;
using VMS.EntityFramework.EntityModel;

namespace VMS.EntityFramework.Data
{
    public class DbVMSInitializer
    {
        private static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            return (hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
        }

        public static void Initialize(DbVMSDataContext context)
        {
            // Seed Departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Name = "Human Resources" },
                    new Department { Name = "IT Department" },
                    new Department { Name = "Finance" }
                );
                context.SaveChanges();
            }

            // Seed Roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Admin", Description = "Full access to all features" },
                    new Role { Name = "Receptionist", Description = "Check-in/out and appointment management" },
                    new Role { Name = "Staff", Description = "View own appointments and dashboard" }
                );
                context.SaveChanges();
            }

            // Seed Staff with hashed passwords
            if (!context.Staffs.Any())
            {
                var hrDept = context.Departments.First(d => d.Name == "Human Resources");
                var itDept = context.Departments.First(d => d.Name == "IT Department");
                var adminRole = context.Roles.First(r => r.Name == "Admin");
                var receptionRole = context.Roles.First(r => r.Name == "Receptionist");
                var staffRole = context.Roles.First(r => r.Name == "Staff");

                var (adminHash, adminSalt) = HashPassword("Admin@123");
                var (receptionHash, receptionSalt) = HashPassword("Reception@123");
                var (staffHash, staffSalt) = HashPassword("Staff@123");

                context.Staffs.AddRange(
                    new Staff
                    {
                        Name = "Admin User",
                        Email = "admin@vms.com",
                        Phone = "123-456-7890",
                        DepartmentId = itDept.Id,
                        RoleId = adminRole.Id,
                        PasswordHash = adminHash,
                        PasswordSalt = adminSalt,
                    },
                    new Staff
                    {
                        Name = "Sarah (Reception)",
                        Email = "sarah@vms.com",
                        Phone = "123-456-7891",
                        DepartmentId = hrDept.Id,
                        RoleId = receptionRole.Id,
                        PasswordHash = receptionHash,
                        PasswordSalt = receptionSalt,
                    },
                    new Staff
                    {
                        Name = "Alice (Staff)",
                        Email = "alice@vms.com",
                        Phone = "987-654-3210",
                        DepartmentId = itDept.Id,
                        RoleId = staffRole.Id,
                        PasswordHash = staffHash,
                        PasswordSalt = staffSalt,
                    }
                );
                context.SaveChanges();
            }

            // Seed Visitors
            if (!context.Visitors.Any())
            {
                context.Visitors.AddRange(
                    new Visitor { FirstName = "Michael", LastName = "Johnson", Email = "michael@example.com", Phone = "555-123-4567", Company = "ABC Corp", IdentificationNumber = "ID12345" },
                    new Visitor { FirstName = "Emma", LastName = "Brown", Email = "emma@example.com", Phone = "555-987-6543", Company = "XYZ Ltd", IdentificationNumber = "ID67890" }
                );
                context.SaveChanges();
            }
        }
    }
}

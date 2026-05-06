using System.Security.Cryptography;
using System.Text;
using VMS.EntityFramework.EntityModel;

namespace VMS.EntityFramework.Data
{
    public class DbVMSInitializer
    {
        // SHA256 to match AuthRepository and StaffRepository
        private static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            using var hmac = new HMACSHA256();
            return (hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
        }

        public static void Initialize(DbVMSDataContext context)
        {
            // Seed Departments
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { Name = "Human Resources", ChangedBy = 1, ChangedByName = "System" },
                    new Department { Name = "IT Department", ChangedBy = 1, ChangedByName = "System" },
                    new Department { Name = "Finance", ChangedBy = 1, ChangedByName = "System" }
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

            // Seed Staff
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
                        Phone = "1234567890",
                        DepartmentId = itDept.Id,
                        RoleId = adminRole.Id,
                        PasswordHash = adminHash,
                        PasswordSalt = adminSalt,
                    },
                    new Staff
                    {
                        Name = "Sarah Reception",
                        Email = "sarah@vms.com",
                        Phone = "1234567891",
                        DepartmentId = hrDept.Id,
                        RoleId = receptionRole.Id,
                        PasswordHash = receptionHash,
                        PasswordSalt = receptionSalt,
                    },
                    new Staff
                    {
                        Name = "Alice Staff",
                        Email = "alice@vms.com",
                        Phone = "9876543210",
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
                    new Visitor { FirstName = "Michael", LastName = "Johnson", Email = "michael@example.com", Phone = "5551234567", Address = "", Company = "ABC Corp", IdentificationNumber = "ID12345", ChangeBy = 1 },
                    new Visitor { FirstName = "Emma", LastName = "Brown", Email = "emma@example.com", Phone = "5559876543", Address = "", Company = "XYZ Ltd", IdentificationNumber = "ID67890", ChangeBy = 1 }
                );
                context.SaveChanges();
            }
        }
    }
}
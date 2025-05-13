using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.EntityModel;

namespace VMS.EntityFramework
{
    public class DbVMSDataContext : DbContext
    {
        public DbVMSDataContext(DbContextOptions<DbVMSDataContext> options) : base(options) { }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Staff> Staffs { get; set;}

        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Department>().ToTable("Department");
        //    //base.OnModelCreating(modelBuilder);

        //}
    }
}

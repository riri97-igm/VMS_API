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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Department>().ToTable("Department");
        //    //base.OnModelCreating(modelBuilder);

        //}
    }
}

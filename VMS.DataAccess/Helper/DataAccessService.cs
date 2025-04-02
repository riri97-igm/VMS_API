using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.DataAccess;
using VMS.DataAccess.Implementation;
using VMS.DataAccess.Interface;


namespace VMS.DataAccess.Helper
{
    public class DataAccessService
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration Configuration
        )
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.EntityFramework.Data;

namespace VMS.EntityFramework.Helper
{
    public static class DbVMSService
    {      
        public static void ConfigureServices(IServiceCollection services,IConfiguration Configuration, string connectionString)
        {            
            services.AddDbContext<DbVMSDataContext>(options =>
                options.UseSqlServer($"name=ConnectionStrings:{connectionString}"));
            //services.AddDbContext<DbVMSDataContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString(connectionString)));

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public static void MigrateDatabase(IServiceScope scope)
        {
            var dbContextOptions = scope.ServiceProvider.GetRequiredService<DbVMSDataContext>();            
            dbContextOptions.Database.Migrate();
            DbVMSInitializer.Initialize(dbContextOptions);
        }
    }
}

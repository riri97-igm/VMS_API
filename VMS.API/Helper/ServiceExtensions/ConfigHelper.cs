﻿using VMS.DataAccess.Helper;
using VMS.EntityFramework.Helper;

namespace VMS.API.Helper.ServiceExtensions
{
    public class ConfigHelper
    {
        public static void ConfigureService(WebApplicationBuilder builder)
        {
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            // Configure logging(write to both file and Seq)
            //LoggerHelper.Instance.ConfigureLogging("http://localhost:5341", logFilePath);

            //**Register service for EF Core Dependencies
            DbVMSService.ConfigureServices(builder.Services, builder.Configuration, builder.Configuration.GetSection("ConnectionStrings").GetChildren().First().Key);

            ////Register Media Configuration Settings
            //builder.Services.Configure<MediaSettings>(builder.Configuration);

            //**Register service for DataAccess Dependencies
            DataAccessService.ConfigureServices(builder.Services, builder.Configuration);
        }


        // Make DB to EnsureCreated, and Initize Default Data
        public static void MigrateDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                //DbKiloTaxiService.DbEnsureCreate(scope);
                DbVMSService.MigrateDatabase(scope);
            }
        }
    }
}

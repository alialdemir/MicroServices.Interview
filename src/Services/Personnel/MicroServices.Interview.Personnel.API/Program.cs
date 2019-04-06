using MicroServices.Interview.Personnel.API.Extensions;
using MicroServices.Interview.Personnel.API.Infrastructure;
using MicroServices.Interview.Personnel.API.Migrations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;

namespace MicroServices.Interview.Personnel.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                         .MigrateDatabase((services, updateDatabase) =>// Database migration işlemi yapırouz ve daha sonra tabloya default kayıtlar ekliyoruz
                         {
                             var settings = services.GetService<IOptions<PersonnelSettings>>();

                             if (settings != null)
                             {
                                 var logger = services.GetService<ILogger<PersonnelContextSeed>>();

                                 Assembly[] migrationsAssemblies = new Assembly[]// fluent migration assembly üzerinden execute ettiği için oluşturduğumuz migration sınıfının assembly'sini alıyoruz
                                                                   {
                                                                         typeof(FirstMigration).Assembly,
                                                                   };

                                 DapperExtensions.DapperExtensions.SetMappingAssemblies(migrationsAssemblies);// burada dapper'da tanımlamamız gerekiyor aksi halde repository base sınıfı tabloyu tanımıyor

                                 updateDatabase(
                                     settings.Value.ConnectionString,// connection string env dan alıyoruz
                                     migrationsAssemblies,
                                     () =>// eğer migration işlemi başarılı olursa veritabanına default kayıtları eklemesi için seed kısmını çağrıyoruz
                                     {
                                         new PersonnelContextSeed(settings.Value.ConnectionString, logger)
                                         .InsertDataAsync()
                                         .Wait();
                                     });
                             }
                         })
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                    config.AddEnvironmentVariables();
                });
    }
}
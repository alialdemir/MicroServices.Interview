using IdentityServer4.EntityFramework.DbContexts;
using MicroServices.Interview.Identity.API.Data;
using MicroServices.Interview.Identity.API.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroServices.Interview.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDatabase<PersistedGrantDbContext>((_, __) => { })
                .MigrateDatabase<ApplicationDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();

                    new ApplicationDbContextSeed()
                        .SeedAsync(context, env, logger)
                        .Wait();
                })
                .MigrateDatabase<ConfigurationDbContext>((context, services) =>
                {
                    var configuration = services.GetService<IConfiguration>();

                    new ConfigurationDbContextSeed()
                        .SeedAsync(context, configuration)
                        .Wait();
                }).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
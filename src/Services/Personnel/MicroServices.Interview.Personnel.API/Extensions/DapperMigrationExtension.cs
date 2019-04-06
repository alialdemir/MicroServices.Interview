using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Reflection;

namespace MicroServices.Interview.Personnel.API.Extensions
{
    /// <summary>
    /// Bu sınıf ayrı bir core katmana alınıp ileride eklenebilecek diğer micro servislerinde kullanması sağlanabilir
    /// </summary>
    public static class DapperMigrationExtension
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost, Action<IServiceProvider, Action<string, Assembly[], Action>> seeder)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<IWebHost>>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(DapperMigrationExtension).Name}");

                    var retry = GetRetryPolicy();

                    // Olası bir bağlantı kopması yada containerlerin geç ayağa kalması gibi sorunlar için 3 sefer denetiyoruz.
                    retry.Execute(() =>
                    {
                        seeder(services,
                      (connectionString, assemblies, databaseSeeder) =>
                      {
                          UpdateDatabaseAsync(services, connectionString, databaseSeeder, assemblies);
                      });
                    });

                    logger.LogInformation($"Migrated database associated with context {typeof(DapperMigrationExtension).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(DapperMigrationExtension).Name}");
                }
            }

            return webHost;
        }

        /// <summary>
        /// Tekrar sayısı ve sürelerini ile ilgili bilgileri döndürür
        /// 10 sn arayla 3 tekrar yapar
        /// </summary>
        /// <returns></returns>
        private static RetryPolicy GetRetryPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetry(new TimeSpan[]
                         {
                             TimeSpan.FromSeconds(10),
                             TimeSpan.FromSeconds(20),
                             TimeSpan.FromSeconds(30),
                         });
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private static IServiceProvider CreateServices(string connectionString, params Assembly[] assemblies)
        {
            return new ServiceCollection()
                // FluentMigrator services eklendi
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Hangi migration sınıflarını çalıştıracağını eklendi
                    .ScanIn(assemblies).For.Migrations())
                // migration yaparken log bassın
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Veritabanında migration işlemi gerçekleştirir
        /// </sumamry>
        private static void UpdateDatabaseAsync(IServiceProvider services, string connectionString, Action databaseSeeder, params Assembly[] assemblies)
        {
            var retry = GetRetryPolicy();

            var logger = services.GetRequiredService<ILogger<IWebHost>>();

            try
            {
                retry.Execute(() =>
                {
                    logger.LogInformation($"Migrating database associated with context {nameof(UpdateDatabaseAsync)}");

                    // runner instance oluşturuldu
                    var runner = CreateServices(connectionString, assemblies).GetRequiredService<IMigrationRunner>();

                    // Migrations çalıştırıldı
                    runner.MigrateUp();
                    databaseSeeder();

                    logger.LogInformation($"Migrated database associated with context {nameof(UpdateDatabaseAsync)}");
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database used on context {nameof(UpdateDatabaseAsync)}");
            }
        }
    }
}
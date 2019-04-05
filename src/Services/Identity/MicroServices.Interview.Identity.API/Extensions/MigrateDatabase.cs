using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;

namespace MicroServices.Interview.Identity.API.Extensions
{
    /// <summary>
    /// Bu sınıf ayrı bir core katmana alınıp ileride eklenebilecek diğer micro servislerinde kullanması sağlanabilir
    /// </summary>
    public static class IWebHostExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                    var retry = GetRetryPolicy();

                    // Olası bir bağlantı kopması yada containerlerin geç ayağa kalması gibi sorunlar için 3 sefer denetiyoruz.
                    retry.Execute(() =>
                    {
                        context.Database
                        .Migrate();// TODO: bazen docker sql tarafını geç başlatığı için sorun çıkarabiliyor çözüm üretilmeli

                        seeder(context, services);
                    });

                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
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
    }
}
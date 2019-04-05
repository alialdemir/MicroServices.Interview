using MicroServices.Interview.Identity.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices.Interview.Identity.API.Data
{
    public class ApplicationDbContextSeed
    {
        #region Private variables

        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        #endregion Private variables

        #region methods

        /// <summary>
        /// User tablosuna default kullanıcıları ekler
        /// </summary>
        /// <param name="context"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    context.Users.AddRange(GetDefaultUser());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex.Message, $"There is an error migrating data for ApplicationDbContext");

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        /// <summary>
        /// Default kullanıcıları oluşturur
        /// </summary>
        /// <returns>Kullanıcı listesi</returns>
        private IEnumerable<ApplicationUser> GetDefaultUser()
        {
            var aliUser =
            new ApplicationUser()
            {
                PhoneNumber = "1234567890",
                UserName = "witcherfearless",
                Email = "aldemirali93@gmail.com",
                NormalizedEmail = "ALDEMIRALI93@GMAIL.COM",
                NormalizedUserName = "WITCHERFEARLESS",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            aliUser.PasswordHash = _passwordHasher.HashPassword(aliUser, "12345678");

            var adminUser =
            new ApplicationUser()
            {
                PhoneNumber = "1234567890",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "DEMO@DEMO.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "admin");

            return new List<ApplicationUser>()
            {
               aliUser,
               adminUser
            };
        }

        #endregion methods
    }
}
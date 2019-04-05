﻿using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MicroServices.Interview.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroServices.Interview.Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        #region Private variables

        private readonly UserManager<ApplicationUser> _userManager;

        #endregion Private variables

        #region Constructor

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        #endregion Constructor

        #region Methods

        async public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
                throw new ArgumentException("Invalid subject identifier");

            var claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();
        }

        /// <summary>
        /// token aktif mi kontrol eder
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async public Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId);

            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                            return;
                    }
                }

                context.IsActive =
                    !user.LockoutEnabled ||
                    !user.LockoutEnd.HasValue ||
                    user.LockoutEnd <= DateTime.Now;
            }
        }

        /// <summary>
        /// Token içerisinde döndürülecek kullanıcı bilgileri
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            // TODO: Role bilgileri
            return claims;
        }

        #endregion Methods
    }
}
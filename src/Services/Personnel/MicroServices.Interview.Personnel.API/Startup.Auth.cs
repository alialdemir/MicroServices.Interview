using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace MicroServices.Interview.Personnel.API
{
    public partial class Startup
    {
        public IServiceCollection AddAuth(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = Configuration.GetValue<string>("IdentityUrl");// identity server url
            var audience = Configuration.GetValue<string>("Audience");// hangi servis olduğunu yazıyoruz  identity servisinde config classında tanımladığımız api adı olmalı

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;// token için bu url gideceğini tanımladık
                options.RequireHttpsMetadata = false;// ssl olmasın
                options.Audience = audience;// Hangi servis olduğunu yazıyoruz
            });

            return services;
        }

        public IApplicationBuilder AddAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();

            return app;
        }
    }
}
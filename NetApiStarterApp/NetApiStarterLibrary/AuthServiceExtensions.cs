using System;
using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetApiStarterLibrary.Models;

namespace NetApiStarterLibrary
{
	public static class AuthServiceExtensions
	{
        public static IdentityBuilder ConfigureIdentity<T>(this IServiceCollection services) where T : DbContext
        {
            var builder = services.AddIdentity<ApiUser, ApiRole>(q =>
            {
                q.User.RequireUniqueEmail = true;
                q.Password.RequireDigit = false;
                q.Password.RequiredLength = 6;
                q.Password.RequireUppercase = false;
                q.Password.RequireLowercase = false;
                q.Password.RequireNonAlphanumeric = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(ApiRole), services);
            builder.AddEntityFrameworkStores<T>().AddDefaultTokenProviders();
            return builder;
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("NetApiStarterLibaryConfig");
            var jwtKey = Configuration.GetSection("JWT_KEY").Value;

            if(jwtKey == null)
            {
                jwtKey = AuthConstants.GetDefaultJwtKey();
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                ValidAudience = jwtSettings.GetSection("Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                RequireExpirationTime = false,
                //LifetimeValidator = CustomLifetimeValidator,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o => {
                    o.SaveToken = true;
                    o.TokenValidationParameters = tokenValidationParameters;
                });
        
        }

        private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}


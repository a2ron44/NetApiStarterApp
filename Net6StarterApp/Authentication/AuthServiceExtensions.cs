using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Net6StarterApp.Authentication.Data;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Permissions;
using Net6StarterApp.Data;

namespace Net6StarterApp.Authentication
{
	public static class AuthServiceExtensions
	{
        public static IdentityBuilder ConfigureIdentity<T>(this IServiceCollection services) where T : DbContext
        {
            var builder = services.AddIdentityCore<ApiUser>(q =>
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
            var jwtSettings = Configuration.GetSection("JwtConfig");
            var jwtKey = Configuration.GetSection("JWT_KEY").Value;

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o => {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                        ValidAudience = jwtSettings.GetSection("Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
        
        }
    }
}


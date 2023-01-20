using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net6StarterApp.Authentication.Data;
using Net6StarterApp.Authentication.Models;
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
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<T>().AddDefaultTokenProviders();
            return builder;
        }

    }
}


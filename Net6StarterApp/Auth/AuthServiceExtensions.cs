//using System;
//using Microsoft.AspNetCore.Identity;
//using Net6StarterApp.Auth.Models;
//using Net6StarterApp.Data;

//namespace Net6StarterApp.Auth
//{
//	public static class AuthServiceExtensions
//	{
//        public static IdentityBuilder ConfigureIdentity(this IServiceCollection services)
//        {
//            var builder = services.AddIdentityCore<ApiUser>(q =>
//                {
//                    q.User.RequireUniqueEmail = true;
//                }
//            );

//            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
//            builder.AddEntityFrameworkStores<ApiDbContext>().AddDefaultTokenProviders();
//            return builder;
//        }

//    }
//}


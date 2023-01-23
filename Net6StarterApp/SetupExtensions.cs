using System;
using Microsoft.OpenApi.Models;

namespace Net6StarterApp
{
    public static class SetupExtensions
    {
        public static void AddSwaggerDocs(this IServiceCollection services, IConfiguration Configuration)
        {
            var appName = Configuration.GetSection("AppDefaults:AppName").Value;

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization Scheme Enter 'Bearer [space]' ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type= ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "0auth2",
                        Name="Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                    }
                 });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = appName, Version = "v1" });
            });
        }
    }
}


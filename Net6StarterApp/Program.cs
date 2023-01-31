
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net6StarterApp.Data;
using Npgsql;
using Net6StarterApp.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Net6StarterApp.Models;
using Net6StarterApp.Authentication.Data;
using Net6StarterApp.Authentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Net6StarterApp.Authentication.Services;
using Net6StarterApp;
using Microsoft.AspNetCore.Authorization;
using Net6StarterApp.Authentication.Permissions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApiDbContext>(options =>
               options.UseNpgsql(builder.Configuration["DbConnection"])
            );


builder.Services.AddAuthentication();
//custom extension for Auth setup
builder.Services.ConfigureIdentity<ApiDbContext>();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
//end auth

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddControllers();

//make sure errors go to API Response object
builder.Services.AddMvc()
       .ConfigureApiBehaviorOptions(opt
           =>
       {
           opt.InvalidModelStateResponseFactory = actionContext =>
           {
              return ApiMessageHandler.CustomModelstateErrorResponse(actionContext);
           };
       });


builder.Services.AddCors(p => p.AddPolicy("corsPolicy", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSwaggerDocs(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy");
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
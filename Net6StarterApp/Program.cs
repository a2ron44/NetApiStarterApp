
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NetApiStarterLibrary.Permissions;
using NetApiStarterLibrary;
using NetApiStarterLibrary.Data;
using NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Services;
using NetApiStarterApp.Data;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
    Console.WriteLine("  {0} = {1}", de.Key, de.Value);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["DbConnection"]);
    options.UseSnakeCaseNamingConvention();
}
);

builder.Services.AddAuthentication();

//custom extension for NetApiStarterLibrary
builder.Services.ConfigureIdentity<ApiDbContext>();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
builder.Services.AddSwaggerDocs(builder.Configuration);
//end NetApiStarterLibrary config

builder.Services.AddControllers();
builder.Services.AddCors(p => p.AddPolicy("corsPolicy", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

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
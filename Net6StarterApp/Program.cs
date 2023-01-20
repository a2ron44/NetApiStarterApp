
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

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<JwtUtils>();
//builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ApiDbContext>(options =>
               options.UseNpgsql(builder.Configuration["DbConnection"])
            );

builder.Services.AddAuthentication();

//custom extension for Auth setup
builder.Services.ConfigureIdentity<ApiDbContext>();

builder.Services.AddControllers();

builder.Services.AddMvc()
       .ConfigureApiBehaviorOptions(opt
           =>
       {
          // opt.SuppressModelStateInvalidFilter = true;
           opt.InvalidModelStateResponseFactory = actionContext =>
           {
              return ApiMessageHandler.CustomModelstateErrorResponse(actionContext);
           };
       });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
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

app.UseCors("corsapp");
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


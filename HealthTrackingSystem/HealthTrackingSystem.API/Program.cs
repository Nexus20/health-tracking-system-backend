using HealthTrackingSystem.API;
using HealthTrackingSystem.API.Extensions;
using HealthTrackingSystem.API.Middlewares;
using HealthTrackingSystem.Application;
using HealthTrackingSystem.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.WithProperty("Application", "Health tracking system")
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(o =>
    o.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Host.UseSerilog((context, config) => config
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.SetupIdentity();

app.Run();

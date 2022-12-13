using System.Reflection;
using HealthTrackingSystem.Application.Interfaces.Infrastructure;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Mqtt;
using HealthTrackingSystem.Infrastructure.Auth;
using HealthTrackingSystem.Infrastructure.Identity;
using HealthTrackingSystem.Infrastructure.Persistence;
using HealthTrackingSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HealthTrackingSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<AppUser, AppRole>()
            .AddUserStore<UserStore<AppUser, AppRole, ApplicationDbContext, string, IdentityUserClaim<string>, AppUserRole,
                IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>()
            .AddRoleStore<RoleStore<AppRole, ApplicationDbContext, string, AppUserRole, IdentityRoleClaim<string>>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddUserManager<UserManager<AppUser>>();
        
        services.AddScoped<IHealthMeasurementsContext, HealthMeasurementsContext>();
        services.AddScoped<IMongoHealthMeasurementRepository, MongoHealthMeasurementRepository>();
        
        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IHospitalAdministratorRepository, HospitalAdministratorRepository>();
        services.AddScoped<IPatientCaretakerRepository, PatientCaretakerRepository>();
        
        services.AddScoped<IIdentityInitializer, IdentityInitializer>();
        services.AddScoped<ICacheToDbDataTransferService, CacheToDbDataTransferService>();
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<JwtHandler>();
        //
        // services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        // services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<IIotSubscribersPool, MqttSubscribersPool>();

        var redisIp = configuration.GetValue<string>("CacheSettings:Ip"); 
        var redisPort = configuration.GetValue<string>("CacheSettings:Port"); 
        var multiplexer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints = { redisIp, redisPort }                
            });
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        return services;
    }
}
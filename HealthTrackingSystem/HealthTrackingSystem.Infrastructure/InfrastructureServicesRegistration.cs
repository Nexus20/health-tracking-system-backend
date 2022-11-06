using System.Reflection;
using HealthTrackingSystem.Application.Interfaces.Infrastructure;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Infrastructure.Identity;
using HealthTrackingSystem.Infrastructure.Persistence;
using HealthTrackingSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IHospitalAdministratorRepository, HospitalAdministratorRepository>();
        
        services.AddScoped<IIdentityInitializer, IdentityInitializer>();
        //
        // services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        // services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
using HealthTrackingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthTrackingSystem.Infrastructure;

public static class InfrastructureServicesRegistration
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        // services.AddScoped<IOrderRepository, OrderRepository>();
        //
        // services.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
        // services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
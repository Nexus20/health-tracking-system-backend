using HealthTrackingSystem.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthTrackingSystem.API.Extensions;

public static class HostExtensions
{
    public static IHost SetupIdentity(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<IIdentityInitializer>();
        initializer.InitializeIdentityData();

        return host;
    }
}
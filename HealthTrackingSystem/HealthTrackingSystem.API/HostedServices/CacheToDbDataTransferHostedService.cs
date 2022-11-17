using HealthTrackingSystem.Application.Interfaces.Infrastructure;

namespace HealthTrackingSystem.API.HostedServices;

public class CacheToDbDataTransferHostedService : BackgroundService
{
    private readonly ILogger<CacheToDbDataTransferHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CacheToDbDataTransferHostedService(ILogger<CacheToDbDataTransferHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dataTransferService = scope.ServiceProvider.GetRequiredService<ICacheToDbDataTransferService>();
                    await dataTransferService.TransferDataAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception was thrown while transfering data from cache to db");
        }
    }
}
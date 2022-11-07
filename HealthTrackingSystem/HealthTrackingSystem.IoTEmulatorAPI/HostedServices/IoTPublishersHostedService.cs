using HealthTrackingSystem.IoTEmulatorAPI.IoT;
using ILogger = Serilog.ILogger;

namespace HealthTrackingSystem.IoTEmulatorAPI.HostedServices;

public class IoTPublishersHostedService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IIotPublishersPool _iotPublishersPool;

    public IoTPublishersHostedService(ILogger logger, IIotPublishersPool iotPublishersPool)
    {
        _logger = logger;
        _iotPublishersPool = iotPublishersPool;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _iotPublishersPool.InitPublishersForPatientsAsync();
            await _iotPublishersPool.ConnectAllAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await _iotPublishersPool.PublishAsync();
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }

            await _iotPublishersPool.DisconnectAllAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An exception was thrown while establishing MQTT subscribers");
        }
    }
}
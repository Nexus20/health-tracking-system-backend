using HealthTrackingSystem.Application.Mqtt;
using ILogger = Serilog.ILogger;

namespace HealthTrackingSystem.API.HostedServices;

public class MqttSubscriberHostedService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IMqttSubscribersPool _mqttSubscribersPool;

    public MqttSubscriberHostedService(ILogger logger, IMqttSubscribersPool mqttSubscribersPool)
    {
        _logger = logger;
        _mqttSubscribersPool = mqttSubscribersPool;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _mqttSubscribersPool.InitSubscribersForPatientsAsync();
            await _mqttSubscribersPool.ConnectAllAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
            }

            await _mqttSubscribersPool.DisconnectAllAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An exception was thrown while establishing MQTT subscribers");
        }
    }
}
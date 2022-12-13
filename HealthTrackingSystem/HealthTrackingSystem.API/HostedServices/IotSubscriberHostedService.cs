using System;
using System.Threading;
using System.Threading.Tasks;
using HealthTrackingSystem.Application.Mqtt;
using Microsoft.Extensions.Hosting;
using ILogger = Serilog.ILogger;

namespace HealthTrackingSystem.API.HostedServices;

public class IotSubscriberHostedService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IIotSubscribersPool _mqttSubscribersPool;

    public IotSubscriberHostedService(ILogger logger, IIotSubscribersPool mqttSubscribersPool)
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
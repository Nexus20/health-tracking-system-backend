using HealthTrackingSystem.API.Hubs;
using HealthTrackingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HealthTrackingSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartController : ControllerBase
{
    private readonly IHubContext<HealthMeasurementsHub> _hub;
    private readonly EcgTimerManager _ecgTimer;
    private readonly HeartRateTimerManager _heartRateTimer;
        
    public ChartController(IHubContext<HealthMeasurementsHub> hub, EcgTimerManager ecgTimer, HeartRateTimerManager heartRateTimer)
    {
        _hub = hub;
        _ecgTimer = ecgTimer;
        _heartRateTimer = heartRateTimer;
    }

    [HttpGet]
    public IActionResult Get()
    {
        if (!_ecgTimer.IsTimerStarted)
            _ecgTimer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferChartData", DataManager.GetData()));
        return Ok(new { Message = "Request Completed" });
    }

    [HttpGet("[action]")]
    public IActionResult GetEcg()
    {
        if (!_ecgTimer.IsTimerStarted)
            _ecgTimer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferEcgData", EcgDataManager.GetPoint()));
        return Ok(new { Message = "GetEcg Request Completed" });
    }

    [HttpGet("[action]")]
    public IActionResult GetHeartRate()
    {
        if (!_heartRateTimer.IsTimerStarted)
            _heartRateTimer.PrepareTimer(() => _hub.Clients.All.SendAsync("TransferHeartRateData", new HeartRateModel()
            {
                HeartRate = new Random().Next(70, 90)
            }));
        return Ok(new { Message = "GetHeartRate Request Completed" });
    }
}
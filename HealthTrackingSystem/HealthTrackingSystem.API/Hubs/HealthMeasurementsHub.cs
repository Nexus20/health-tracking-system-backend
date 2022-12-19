using Microsoft.AspNetCore.SignalR;

namespace HealthTrackingSystem.API.Hubs;

public class HealthMeasurementsHub : Hub
{
    // public async IAsyncEnumerable<EcgPoint> TransferEcgData([EnumeratorCancellation] CancellationToken cancellationToken)
    // {
    //     while (!cancellationToken.IsCancellationRequested)
    //     {
    //         yield return EcgDataManager.GetPoint();
    //         await Task.Delay(1000, cancellationToken);
    //     }
    // }
}
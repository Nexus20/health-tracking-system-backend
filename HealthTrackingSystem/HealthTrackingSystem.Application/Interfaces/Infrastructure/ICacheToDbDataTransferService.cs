namespace HealthTrackingSystem.Application.Interfaces.Infrastructure;

public interface ICacheToDbDataTransferService
{
    public Task TransferDataAsync();
}
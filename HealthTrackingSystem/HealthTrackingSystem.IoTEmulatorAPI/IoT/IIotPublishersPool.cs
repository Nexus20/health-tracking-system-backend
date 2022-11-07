namespace HealthTrackingSystem.IoTEmulatorAPI.IoT;

public interface IIotPublishersPool
{
    Task InitPublishersForPatientsAsync();
    Task ConnectAllAsync();
    Task DisconnectAllAsync();
    void AddNewPublisher(string patientId);
    Task ConnectOneAsync(string patientId);
    Task DisconnectOneAsync(string patientId);

    Task PublishAsync();
}
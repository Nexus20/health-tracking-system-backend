namespace HealthTrackingSystem.Application.Mqtt;

public interface IIotSubscribersPool
{
    Task InitSubscribersForPatientsAsync();
    Task ConnectAllAsync();
    Task DisconnectAllAsync();
    void AddNewSubscriber(string patientId);
    Task ConnectOneAsync(string patientId);
    Task DisconnectOneAsync(string patientId);
}
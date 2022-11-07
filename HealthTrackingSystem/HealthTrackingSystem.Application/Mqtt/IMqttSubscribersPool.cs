namespace HealthTrackingSystem.Application.Mqtt;

public interface IMqttSubscribersPool
{
    Task InitSubscribersForPatientsAsync();
    Task ConnectAllAsync();
    Task DisconnectAllAsync();
    void AddNewSubscriber(string patientId);
    Task ConnectOneAsync(string patientId);
    Task DisconnectOneAsync(string patientId);
}
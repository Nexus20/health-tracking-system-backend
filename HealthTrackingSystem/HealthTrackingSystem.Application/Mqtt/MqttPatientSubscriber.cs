using MQTTnet.Client;

namespace HealthTrackingSystem.Application.Mqtt;

public class MqttPatientSubscriber
{
    public IMqttClient MqttClient { get; set; } = null!;
    public string PatientId { get; set; } = null!;

    public MqttPatientSubscriber()
    {
        
    }

    public MqttPatientSubscriber(IMqttClient mqttClient, string patientId)
    {
        MqttClient = mqttClient;
        PatientId = patientId;
    }
}
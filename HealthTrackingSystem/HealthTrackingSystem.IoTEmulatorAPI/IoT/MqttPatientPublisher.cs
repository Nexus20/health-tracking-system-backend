using MQTTnet.Client;

namespace HealthTrackingSystem.IoTEmulatorAPI.IoT;

public class MqttPatientPublisher
{
    public IMqttClient MqttClient { get; set; } = null!;
    public string PatientId { get; set; } = null!;

    public MqttPatientPublisher()
    {
        
    }

    public MqttPatientPublisher(IMqttClient mqttClient, string patientId)
    {
        MqttClient = mqttClient;
        PatientId = patientId;
    }
}
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Dtos;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;

namespace HealthTrackingSystem.IoTEmulatorAPI.IoT;

public class MqttPublishersPool : IIotPublishersPool
{
    private readonly List<MqttPatientPublisher> _publishers = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly Serilog.ILogger _logger;
    private readonly IConfiguration _configuration;

    public MqttPublishersPool(IServiceProvider serviceProvider, Serilog.ILogger logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task InitPublishersForPatientsAsync()
    {
        List<PatientResult> allPatients;
        using (var scope = _serviceProvider.CreateScope()) {
            var patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
            allPatients = await patientService.GetAsync(new GetPatientsRequest());
        }

        var patientsWithoutPublishers = allPatients
            .Where(patient => _publishers.All(x => x.PatientId != patient.Id))
            .ToList();
        
        foreach (var patient in patientsWithoutPublishers)
        {
            AddNewPublisher(patient.Id);
        }
    }

    public async Task ConnectAllAsync()
    {
        foreach (var publisher in _publishers)
        {
            await publisher.MqttClient.ConnectAsync(GetMqttClientOptions());
        }
    }

    public async Task DisconnectAllAsync()
    {
        foreach (var publisher in _publishers)
        {
            await publisher.MqttClient.DisconnectAsync();
        }
    }

    public void AddNewPublisher(string patientId)
    {
        var mqttClient = CreateMqttClient(patientId);
        _publishers.Add(new MqttPatientPublisher(mqttClient, patientId));
    }

    public async Task ConnectOneAsync(string patientId)
    {
        var publisher = _publishers.SingleOrDefault(x => x.PatientId == patientId);

        if (publisher == null)
        {
            throw new InvalidOperationException($"Mqtt Publisher with patient id {patientId} not found");
        }
            
        await publisher.MqttClient.ConnectAsync(GetMqttClientOptions());
    }

    public async Task DisconnectOneAsync(string patientId)
    {
        var publisher = _publishers.SingleOrDefault(x => x.PatientId == patientId);

        if (publisher == null)
        {
            throw new InvalidOperationException($"Mqtt Publisher with patient id {patientId} not found");
        }
            
        await publisher.MqttClient.DisconnectAsync();
    }

    public async Task PublishAsync()
    {
        foreach (var publisher in _publishers)
        {
            await PublishMessageAsync(publisher);
        }
    }
    
    private static async Task PublishMessageAsync(MqttPatientPublisher mqttPatientPublisher)
    {
        var payload = GeneratePayload();
        
        var message = new MqttApplicationMessageBuilder()
            //.WithTopic("esp32/output")
            .WithTopic(mqttPatientPublisher.PatientId)
            .WithPayload(payload)
            .WithAtLeastOnceQoS()
            .Build();

        if (mqttPatientPublisher.MqttClient.IsConnected)
        {
            await mqttPatientPublisher.MqttClient.PublishAsync(message);
        }
    }

    private static string GeneratePayload()
    {
        var random = new Random();

        var heartRateDto = new HealthMeasurementDto()
        {
            Ecg = random.NextDouble() * 1000,
            HeartRate = random.Next(50, 101),
            DateTime = DateTime.UtcNow
        };

        return JsonConvert.SerializeObject(heartRateDto);
    }

    private IMqttClient CreateMqttClient(string patientId)
    {
        // Creates the client object
        var mqttClient = new MqttFactory().CreateMqttClient();

        // Set up handlers
        mqttClient.UseConnectedHandler(e =>
        {
            _logger.Information("Mqtt publisher for patient {PatientId} connected", patientId);
        });

        mqttClient.UseDisconnectedHandler(e =>
        {
            _logger.Information("Mqtt publisher for patient {PatientId} disconnected", patientId);
        });

        return mqttClient;
    }
    
    private IMqttClientOptions GetMqttClientOptions()
    {
        var mqttBrokerIp = _configuration.GetValue<string>("MqttSettings:brokerIp");
        var mqttBrokerPort = _configuration.GetValue<int>("MqttSettings:brokerPort");
        
        return new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer(mqttBrokerIp, mqttBrokerPort)
            .WithCleanSession()
            .Build();
    }
}
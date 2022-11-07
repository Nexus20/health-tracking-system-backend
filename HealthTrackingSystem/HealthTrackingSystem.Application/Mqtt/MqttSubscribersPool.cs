using System.Text;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Serilog;

namespace HealthTrackingSystem.Application.Mqtt;

public class MqttSubscribersPool : IMqttSubscribersPool
{
    private readonly List<MqttPatientSubscriber> _subscribers = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public MqttSubscribersPool(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task InitSubscribersForPatientsAsync()
    {
        List<PatientResult> allPatients;
        using (var scope = _serviceProvider.CreateScope()) {
            var patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
            allPatients = await patientService.GetAsync(new GetPatientsRequest());
        }

        var patientsWithoutSubscribers = allPatients
            .Where(patient => _subscribers.All(x => x.PatientId != patient.Id))
            .ToList();
        
        foreach (var patient in patientsWithoutSubscribers)
        {
            AddNewSubscriber(patient.Id);
        }
    }

    public async Task ConnectAllAsync()
    {
        foreach (var subscriber in _subscribers)
        {
            await subscriber.MqttClient.ConnectAsync(GetMqttClientOptions());
        }
    }

    public async Task ConnectOneAsync(string patientId)
    {
        var subscriber = _subscribers.SingleOrDefault(x => x.PatientId == patientId);

        if (subscriber == null)
        {
            throw new InvalidOperationException($"Mqtt subscriber with patient id {patientId} not found");
        }
            
        await subscriber.MqttClient.ConnectAsync(GetMqttClientOptions());
    }
    
    public async Task DisconnectOneAsync(string patientId)
    {
        var subscriber = _subscribers.SingleOrDefault(x => x.PatientId == patientId);

        if (subscriber == null)
        {
            throw new InvalidOperationException($"Mqtt subscriber with patient id {patientId} not found");
        }
            
        await subscriber.MqttClient.DisconnectAsync();
    }
    
    public async Task DisconnectAllAsync()
    {
        foreach (var subscriber in _subscribers)
        {
            await subscriber.MqttClient.DisconnectAsync();
        }
    }

    public void AddNewSubscriber(string patientId)
    {
        var mqttClient = CreateMqttClient(patientId);
        _subscribers.Add(new MqttPatientSubscriber(mqttClient, patientId));
    }

    private IMqttClient CreateMqttClient(string patientId)
    {
        // Creates the client object
        var mqttClient = new MqttFactory().CreateMqttClient();

        // Set up handlers
        mqttClient.UseConnectedHandler(async e =>
        {
            _logger.Information("Mqtt subscriber for patient {PatientId} connected", patientId);
                
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(patientId)
                .Build();

            await mqttClient.SubscribeAsync(topicFilter);
        });

        mqttClient.UseDisconnectedHandler(e =>
        {
            _logger.Information("Mqtt subscriber for patient {PatientId} disconnected", patientId);
        });

        mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            _logger.Information("Message from subscriber with patient id {PatientId}: {Payload}", patientId, Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
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
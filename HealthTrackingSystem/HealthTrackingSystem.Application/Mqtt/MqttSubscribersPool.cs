using System.Globalization;
using System.Text;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Dtos;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using HealthTrackingSystem.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;

namespace HealthTrackingSystem.Application.Mqtt;

public class MqttSubscribersPool : IIotSubscribersPool
{
    private readonly List<MqttPatientSubscriber> _subscribers = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _redisCache;
    private readonly IConnectionMultiplexer _redis;

    public MqttSubscribersPool(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger, IDistributedCache redisCache, IConnectionMultiplexer redis)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
        _redisCache = redisCache;
        _redis = redis;
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

        mqttClient.UseApplicationMessageReceivedHandler(async e =>
        {
            _logger.Information("Message from subscriber with patient id {PatientId}: {Payload}", patientId, Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
            var stringPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var healthMeasurementDto = JsonConvert.DeserializeObject<HealthMeasurementDto>(stringPayload);

            // _redis.GetDatabase().SetAdd(new RedisKey(patientId), new RedisValue(stringPayload));
            await _redis.GetDatabase().SetAddAsync($"patient:{patientId}", stringPayload);
            // using (var scope = _serviceProvider.CreateScope())
            // {
            //     var healthMeasurementRepository = scope.ServiceProvider.GetRequiredService<IMongoHealthMeasurementRepository>();
            //     
            //     var mongoHealthMeasurement = new MongoHealthMeasurement()
            //     {
            //         Ecg = healthMeasurementDto.Ecg,
            //         HeartRate = healthMeasurementDto.HeartRate,
            //         CreatedDate = healthMeasurementDto.DateTime,
            //         PatientId = patientId
            //     };
            //     
            //     await healthMeasurementRepository.CreateMongoHealthMeasurementAsync(mongoHealthMeasurement);
            // }

            // await _redisCache.C
            // await _redisCache.SetStringAsync(patientId, stringPayload);
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
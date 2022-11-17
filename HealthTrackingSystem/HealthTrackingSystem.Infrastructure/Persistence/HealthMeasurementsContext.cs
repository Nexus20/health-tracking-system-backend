using HealthTrackingSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace HealthTrackingSystem.Infrastructure.Persistence;

public class HealthMeasurementsContext : IHealthMeasurementsContext
{
    public HealthMeasurementsContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("MongoDbSettings:ConnectionString");
        var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");
        var collectionName = configuration.GetValue<string>("MongoDbSettings:CollectionName");
        
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        HealthMeasurements = database.GetCollection<MongoHealthMeasurement>(collectionName);
    }

    public IMongoCollection<MongoHealthMeasurement> HealthMeasurements { get; }
}
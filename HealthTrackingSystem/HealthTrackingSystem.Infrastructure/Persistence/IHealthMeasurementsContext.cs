using HealthTrackingSystem.Domain.Entities;
using MongoDB.Driver;

namespace HealthTrackingSystem.Infrastructure.Persistence;

public interface IHealthMeasurementsContext
{
    IMongoCollection<MongoHealthMeasurement> HealthMeasurements { get; }
}
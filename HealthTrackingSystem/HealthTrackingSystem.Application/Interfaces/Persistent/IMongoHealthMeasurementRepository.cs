using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IMongoHealthMeasurementRepository
{
    Task<List<MongoHealthMeasurement>> GetMongoHealthMeasurements();
    Task<MongoHealthMeasurement> GetMongoHealthMeasurement(string id);
    Task<List<MongoHealthMeasurement>> GetMongoHealthMeasurementByPatientId(string patientId);
    Task CreateMongoHealthMeasurementAsync(MongoHealthMeasurement mongoHealthMeasurement);
    Task<bool> UpdateMongoHealthMeasurement(MongoHealthMeasurement mongoHealthMeasurement);
    Task<bool> DeleteMongoHealthMeasurement(string id);
}
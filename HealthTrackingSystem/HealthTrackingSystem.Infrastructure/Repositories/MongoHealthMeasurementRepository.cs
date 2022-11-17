using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Persistence;
using MongoDB.Driver;

namespace HealthTrackingSystem.Infrastructure.Repositories;

public class MongoHealthMeasurementRepository : IMongoHealthMeasurementRepository
{
    private readonly IHealthMeasurementsContext _context;

    public MongoHealthMeasurementRepository(IHealthMeasurementsContext context)
    {
        _context = context;
    }

    public Task<List<MongoHealthMeasurement>> GetMongoHealthMeasurements()
    {
        return _context.HealthMeasurements.Find(x => true).ToListAsync();
    }

    public Task<MongoHealthMeasurement> GetMongoHealthMeasurement(string id)
    {
        return _context.HealthMeasurements.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<MongoHealthMeasurement>> GetMongoHealthMeasurementByPatientId(string patientId)
    {
        var filter = Builders<MongoHealthMeasurement>.Filter.Eq(p => p.PatientId, patientId);

        return _context.HealthMeasurements.Find(filter).ToListAsync();
    }

    public Task CreateMongoHealthMeasurementAsync(MongoHealthMeasurement mongoHealthMeasurement)
    {
        return _context.HealthMeasurements.InsertOneAsync(mongoHealthMeasurement);
    }

    public async Task<bool> UpdateMongoHealthMeasurement(MongoHealthMeasurement mongoHealthMeasurement)
    {
        var updateResult =
            await _context.HealthMeasurements.ReplaceOneAsync(filter: p => p.Id == mongoHealthMeasurement.Id, replacement: mongoHealthMeasurement);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteMongoHealthMeasurement(string id)
    {
        var filter = Builders<MongoHealthMeasurement>.Filter.Eq(p => p.Id, id);

        var deleteResult = await _context
            .HealthMeasurements
            .DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged
               && deleteResult.DeletedCount > 0;
    }
}
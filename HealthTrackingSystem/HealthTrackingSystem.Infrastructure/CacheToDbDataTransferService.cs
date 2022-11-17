using HealthTrackingSystem.Application.Interfaces.Infrastructure;
using HealthTrackingSystem.Application.Models.Dtos;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HealthTrackingSystem.Infrastructure;

public class CacheToDbDataTransferService : ICacheToDbDataTransferService
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConnectionMultiplexer _redis;

    public CacheToDbDataTransferService(ApplicationDbContext applicationDbContext, IConnectionMultiplexer redis)
    {
        _applicationDbContext = applicationDbContext;
        _redis = redis;
    }

    public async Task TransferDataAsync()
    {
        var patientsIds = await _applicationDbContext.Patients.Select(x => x.Id).ToListAsync();

        if (!patientsIds.Any())
            return;

        var allPatientsHealthMeasurements = new List<HealthMeasurement>();
        
        foreach (var patientId in patientsIds)
        {
            var result = await _redis.GetDatabase().SetMembersAsync($"patient:{patientId}");

            var patientHealthMeasurements = result
                .Select(x => x.ToString())
                .Where(x => x.StartsWith("{"))
                .Select(JsonConvert.DeserializeObject<HealthMeasurementDto>)
                .Where(x => x != null)
                .Select(x => new HealthMeasurement()
                {
                    CreatedDate = x.DateTime,
                    Ecg = x.Ecg,
                    HeartRate = x.HeartRate,
                    PatientId = patientId
                }).ToList();
            
            allPatientsHealthMeasurements.AddRange(patientHealthMeasurements);

            await _redis.GetDatabase().SetRemoveAsync($"patient:{patientId}", result);
        }

        await _applicationDbContext.HealthMeasurements.AddRangeAsync(allPatientsHealthMeasurements);
        await _applicationDbContext.SaveChangesAsync();
    }
}
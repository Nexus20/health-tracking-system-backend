using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IPatientRepository : IAsyncRepository<Patient>
{
    Task AddAsync(Patient patientEntity, User userEntity, string password);
}
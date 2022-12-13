using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IPatientRepository : IRepository<Patient>
{
    Task AddAsync(Patient patientEntity, User userEntity, string password);
}
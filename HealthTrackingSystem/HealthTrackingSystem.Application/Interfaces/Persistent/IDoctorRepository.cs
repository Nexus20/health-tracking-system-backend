using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task AddAsync(Doctor doctorEntity, User userEntity, string password);
}
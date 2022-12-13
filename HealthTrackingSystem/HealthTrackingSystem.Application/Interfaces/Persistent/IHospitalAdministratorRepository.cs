using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IHospitalAdministratorRepository : IRepository<HospitalAdministrator>
{
    Task AddAsync(HospitalAdministrator hospitalAdministratorEntity, User userEntity, string password);
}
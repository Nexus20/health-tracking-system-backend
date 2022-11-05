using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IHospitalRepository : IAsyncRepository<Hospital>
{
    
}
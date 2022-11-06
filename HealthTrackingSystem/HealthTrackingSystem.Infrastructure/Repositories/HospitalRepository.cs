using AutoMapper;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Persistence;

namespace HealthTrackingSystem.Infrastructure.Repositories;

public class HospitalRepository : RepositoryBase<Hospital>, IHospitalRepository
{
    public HospitalRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
    }
}
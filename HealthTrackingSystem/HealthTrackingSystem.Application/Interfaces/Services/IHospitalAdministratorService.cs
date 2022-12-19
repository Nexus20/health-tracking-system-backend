using HealthTrackingSystem.Application.Models.Requests.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IHospitalAdministratorService
{
    public Task<HospitalAdministratorResult> GetByIdAsync(string id);
    public Task<List<HospitalAdministratorResult>> GetAsync(GetHospitalAdministratorsRequest request);
    public Task<HospitalAdministratorResult> CreateAsync(CreateHospitalAdministratorRequest request);
    public Task<HospitalAdministratorResult> UpdateAsync(string id, UpdateHospitalAdministratorRequest request);
    public Task DeleteAsync(string id);
}
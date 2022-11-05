using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Hospitals;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IHospitalService
{
    public Task<HospitalResult> GetByIdAsync(string id);
    public Task<List<HospitalResult>> GetAsync(GetHospitalsRequest request);
    public Task<HospitalResult> CreateAsync(CreateHospitalRequest request);
    public Task<HospitalResult> UpdateAsync(UpdateHospitalRequest request);
    public Task DeleteAsync(string id);
}
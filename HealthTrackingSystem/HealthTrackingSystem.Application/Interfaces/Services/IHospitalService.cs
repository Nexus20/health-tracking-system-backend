using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Hospitals;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IHospitalService
{
    public Task<HospitalResult> GetByIdAsync(string id);
    public Task<List<HospitalResult>> GetAsync(GetHospitalsRequest request);
    public Task<HospitalResult> CreateHospitalAsync(CreateHospitalRequest request);
    public Task<HospitalResult> UpdateHospitalAsync(UpdateHospitalRequest request);
    public Task DeleteHospitalAsync(string id);
}
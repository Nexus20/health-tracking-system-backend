using HealthTrackingSystem.Application.Models.Requests.Doctors;
using HealthTrackingSystem.Application.Models.Results.Doctors;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IDoctorService
{
    public Task<DoctorResult> GetByIdAsync(string id);
    public Task<List<DoctorResult>> GetAsync(GetDoctorsRequest request);
    public Task<DoctorResult> CreateAsync(CreateDoctorRequest request);
    public Task<DoctorResult> UpdateAsync(UpdateDoctorRequest request);
    public Task DeleteAsync(string id);
}
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IPatientService
{
    public Task<PatientResult> GetByIdAsync(string id);
    public Task<List<PatientResult>> GetAsync(GetPatientsRequest request);
    public Task<PatientResult> CreateAsync(CreatePatientRequest request);
    public Task<PatientResult> UpdateAsync(UpdatePatientRequest request);
    public Task DeleteAsync(string id);

    public Task CreateIotDeviceSubscriberForPatientAsync(string id);
}
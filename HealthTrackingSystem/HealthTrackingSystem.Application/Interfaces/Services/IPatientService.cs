using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IPatientService
{
    Task<PatientResult> GetByIdAsync(string id);
    Task<List<PatientResult>> GetAsync(GetPatientsRequest request);
    Task<PatientResult> CreateAsync(CreatePatientRequest request);
    Task<PatientResult> UpdateAsync(string id, UpdatePatientRequest request);
    Task DeleteAsync(string id);
    Task CreateIotDeviceSubscriberForPatientAsync(string id);
    Task<string> AddDoctorToPatientAsync(string id, AddDoctorToPatientRequest request);
    Task<PatientCaretakerResult?> GetPatientCaretakerAsync(string id);
}
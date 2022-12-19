using HealthTrackingSystem.Application.Models.Requests.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IPatientCaretakerService
{
    Task<PatientCaretakerResult> GetByIdAsync(string id);
    Task<List<PatientCaretakerResult>> GetAsync(GetPatientCaretakersRequest request);
    Task<PatientCaretakerResult> CreateAsync(CreatePatientCaretakerRequest request);
    Task<PatientCaretakerResult> UpdateAsync(UpdatePatientCaretakerRequest request);
    Task DeleteAsync(string id);
    Task<List<PatientResult>> GetPatientsAsync(string id);
}
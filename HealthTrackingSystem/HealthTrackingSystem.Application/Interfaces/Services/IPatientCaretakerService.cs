using HealthTrackingSystem.Application.Models.Requests.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IPatientCaretakerService
{
    public Task<PatientCaretakerResult> GetByIdAsync(string id);
    public Task<List<PatientCaretakerResult>> GetAsync(GetPatientCaretakersRequest request);
    public Task<PatientCaretakerResult> CreateAsync(CreatePatientCaretakerRequest request);
    public Task<PatientCaretakerResult> UpdateAsync(UpdatePatientCaretakerRequest request);
    public Task DeleteAsync(string id);
}
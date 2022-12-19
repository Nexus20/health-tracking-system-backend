using HealthTrackingSystem.Application.Models.Requests.Doctors;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IDoctorService
{
    Task<DoctorResult> GetByIdAsync(string id);
    Task<List<DoctorResult>> GetAsync(GetDoctorsRequest request);
    Task<DoctorResult> CreateAsync(CreateDoctorRequest request);
    Task<DoctorResult> UpdateAsync(string id, UpdateDoctorRequest request);
    Task DeleteAsync(string id);
    Task<List<PatientResult>> GetPatientsAsync(string id);
}
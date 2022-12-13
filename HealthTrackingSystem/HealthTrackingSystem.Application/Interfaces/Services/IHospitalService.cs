using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IHospitalService
{
    public Task<HospitalResult> GetByIdAsync(string id);
    public Task<List<HospitalResult>> GetAsync(GetHospitalsRequest request);
    public Task<HospitalResult> CreateAsync(CreateHospitalRequest request);
    public Task<HospitalResult> UpdateAsync(string id, UpdateHospitalRequest request);
    public Task DeleteAsync(string id);
    Task<List<HospitalAdministratorResult>> GetAdministratorsAsync(string id);
    Task<List<DoctorResult>> GetDoctorsAsync(string id);
    Task<List<PatientResult>> GetPatientsAsync(string id);
}
using HealthTrackingSystem.Application.Models.Results.Abstract;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.Patients;

namespace HealthTrackingSystem.Application.Models.Results.Users;

public class ProfileResult : BaseResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    
    public DoctorResult? Doctor { get; set; }
    public PatientResult? Patient { get; set; }
    public PatientCaretakerResult? PatientCaretaker { get; set; }
    public HospitalAdministratorResult? HospitalAdministrator { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace HealthTrackingSystem.Application.Models.Requests.Patients;

public class AddDoctorToPatientRequest
{
    [Required] public string DoctorId { get; set; } = null!;
}
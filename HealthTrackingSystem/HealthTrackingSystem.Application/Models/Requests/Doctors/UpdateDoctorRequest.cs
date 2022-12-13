using System.ComponentModel.DataAnnotations;

namespace HealthTrackingSystem.Application.Models.Requests.Doctors;

public class UpdateDoctorRequest
{
    [Required] public string HospitalId { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public string Patronymic { get; set; } = null!;
    [Required] public string Phone { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public DateTime BirthDate { get; set; }
}
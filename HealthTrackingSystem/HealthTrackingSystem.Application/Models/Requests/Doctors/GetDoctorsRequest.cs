using System.ComponentModel.DataAnnotations;

namespace HealthTrackingSystem.Application.Models.Requests.Doctors;

public class GetDoctorsRequest
{
    
}

public class CreateDoctorRequest
{
    [Required] public string HospitalId { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public string Patronymic { get; set; } = null!;
    [Required] public string Phone { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public DateTime BirthDate { get; set; }
    [Required] public string Password { get; set; } = null!; // _QGrXyvcmTD4aVQJ_ for tests
}

public class UpdateDoctorRequest
{
    
}
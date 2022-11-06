using HealthTrackingSystem.Application.Models.Results.Abstract;

namespace HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;

public class HospitalAdministratorResult : BaseResult
{
    public string HospitalId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}
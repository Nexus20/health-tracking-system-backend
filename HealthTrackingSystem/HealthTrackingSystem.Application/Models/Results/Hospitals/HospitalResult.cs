using HealthTrackingSystem.Application.Models.Results.Abstract;

namespace HealthTrackingSystem.Application.Models.Results.Hospitals;

public class HospitalResult : BaseResult
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int DoctorsCount { get; set; }
    public int PatientsCount { get; set; }
}
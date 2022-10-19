using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class Patient : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    public string? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    
    public string HospitalId { get; set; } = null!;
    public Hospital Hospital { get; set; } = null!;
    
    public List<HealthMeasurement>? HealthMeasurements { get; set; }
}
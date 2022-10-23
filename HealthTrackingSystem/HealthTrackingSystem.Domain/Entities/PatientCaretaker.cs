using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class PatientCaretaker : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    
    public List<Patient>? Patients { get; set; }
}
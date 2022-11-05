using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class PatientCaretaker : BaseEntity
{
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    
    public virtual List<Patient>? Patients { get; set; }
}
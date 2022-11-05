using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class Patient : BaseEntity
{
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    public string? DoctorId { get; set; }
    public virtual Doctor? Doctor { get; set; }
    
    public string? PatientCaretakerId { get; set; }
    public virtual PatientCaretaker? PatientCaretaker { get; set; }
    
    public string HospitalId { get; set; } = null!;
    public virtual Hospital Hospital { get; set; } = null!;
    
    public virtual List<HealthMeasurement>? HealthMeasurements { get; set; }
}
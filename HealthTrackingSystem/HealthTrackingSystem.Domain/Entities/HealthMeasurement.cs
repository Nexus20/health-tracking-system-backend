using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class HealthMeasurement : BaseEntity
{
    public double Ecg { get; set; }
    public int HeartRate { get; set; }
    
    public string PatientId { get; set; } = null!;
    public virtual Patient Patient { get; set; } = null!;
}
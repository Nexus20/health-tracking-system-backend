using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class Hospital : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public virtual List<Doctor>? Doctors { get; set; }
    public virtual List<HospitalAdministrator>? Administrators { get; set; }
    public virtual List<Patient>? Patients { get; set; }
}
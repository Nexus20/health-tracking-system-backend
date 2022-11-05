using HealthTrackingSystem.Domain.Entities.Abstract;

namespace HealthTrackingSystem.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    
    public virtual Doctor? Doctor { get; set; }
    public virtual Patient? Patient { get; set; }
    public virtual PatientCaretaker? PatientCaretaker { get; set; }
    public virtual HospitalAdministrator? HospitalAdministrator { get; set; }
}
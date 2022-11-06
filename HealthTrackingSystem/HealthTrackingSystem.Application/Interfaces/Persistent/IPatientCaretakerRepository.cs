using System.Linq.Expressions;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Interfaces.Persistent;

public interface IPatientCaretakerRepository : IAsyncRepository<PatientCaretaker>
{
    Task AddAsync(PatientCaretaker patientCaretakerEntity, string password, List<string>? patientsIds);
    Task<List<PatientCaretakerResult>> GetWithPatientsAsync(Expression<Func<PatientCaretaker, bool>>? predicate = null);
}
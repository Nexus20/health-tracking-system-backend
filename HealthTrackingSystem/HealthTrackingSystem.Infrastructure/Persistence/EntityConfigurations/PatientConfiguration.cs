using HealthTrackingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTrackingSystem.Infrastructure.Persistence.EntityConfigurations;

internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Patient)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Hospital)
            .WithMany(x => x.Patients)
            .HasForeignKey(x => x.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.HealthMeasurements)
            .WithOne(x => x.Patient)
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
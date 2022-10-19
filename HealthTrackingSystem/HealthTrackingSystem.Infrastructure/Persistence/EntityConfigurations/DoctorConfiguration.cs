using HealthTrackingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTrackingSystem.Infrastructure.Persistence.EntityConfigurations;

internal class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Doctor)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Hospital)
            .WithMany(x => x.Doctors)
            .HasForeignKey(x => x.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Patients)
            .WithOne(x => x.Doctor)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
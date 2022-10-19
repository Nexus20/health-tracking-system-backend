using HealthTrackingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTrackingSystem.Infrastructure.Persistence.EntityConfigurations;

internal class HospitalAdministratorConfiguration : IEntityTypeConfiguration<HospitalAdministrator>
{
    public void Configure(EntityTypeBuilder<HospitalAdministrator> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.HospitalAdministrator)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Hospital)
            .WithMany(x => x.Administrators)
            .HasForeignKey(x => x.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
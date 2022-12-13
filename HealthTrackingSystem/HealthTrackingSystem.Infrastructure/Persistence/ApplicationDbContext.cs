using System.Reflection;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Domain.Entities.Abstract;
using HealthTrackingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthTrackingSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserClaim<string>, AppUserRole,
    IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<HospitalAdministrator> HospitalAdministrators { get; set; }
    public DbSet<HealthMeasurement> HealthMeasurements { get; set; }
    public DbSet<PatientCaretaker> PatientCaretakers { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }

    public override int SaveChanges()
    {
        AddInfoBeforeUpdate();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        AddInfoBeforeUpdate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddInfoBeforeUpdate()
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedDate = DateTime.UtcNow;
            }
            ((BaseEntity)entry.Entity).LastModifiedDate = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext))!);
    }
}
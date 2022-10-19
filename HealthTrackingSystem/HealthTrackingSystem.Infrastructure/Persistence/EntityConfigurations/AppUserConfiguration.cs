﻿using HealthTrackingSystem.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthTrackingSystem.Infrastructure.Persistence.EntityConfigurations;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}
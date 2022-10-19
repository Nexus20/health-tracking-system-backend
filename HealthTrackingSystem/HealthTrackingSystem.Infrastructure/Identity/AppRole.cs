using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Identity;

public class AppRole : IdentityRole
{
    public List<AppUserRole> UserRoles { get; set; }
}
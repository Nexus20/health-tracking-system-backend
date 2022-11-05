using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public virtual List<AppUserRole> UserRoles { get; set; }
}
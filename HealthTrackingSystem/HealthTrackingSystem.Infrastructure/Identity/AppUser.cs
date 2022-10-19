using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public List<AppUserRole> UserRoles { get; set; }
}
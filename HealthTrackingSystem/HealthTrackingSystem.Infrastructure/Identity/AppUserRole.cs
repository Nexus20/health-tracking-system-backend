using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Identity;

public class AppUserRole : IdentityUserRole<string> {

    public AppUser User { get; set; }

    public AppRole Role { get; set; }
}
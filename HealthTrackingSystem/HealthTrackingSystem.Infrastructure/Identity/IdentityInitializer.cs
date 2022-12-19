using HealthTrackingSystem.Application.Authorization;
using HealthTrackingSystem.Application.Interfaces.Infrastructure;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Identity;

public class IdentityInitializer : IIdentityInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IRepository<User> _userRepository;

    public IdentityInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IRepository<User> userRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public void InitializeIdentityData()
    {
        InitializeSuperAdminRole().Wait();
        RegisterRoleAsync(CustomRoles.Admin).Wait();
        RegisterRoleAsync(CustomRoles.Moderator).Wait();
        RegisterRoleAsync(CustomRoles.User).Wait();
        RegisterRoleAsync(CustomRoles.Doctor).Wait();
        RegisterRoleAsync(CustomRoles.Patient).Wait();
        RegisterRoleAsync(CustomRoles.HospitalAdministrator).Wait();
        RegisterRoleAsync(CustomRoles.PatientCaretaker).Wait();
    }
    
    private async Task<AppRole> RegisterRoleAsync(string roleName)
    {

        var role  = await _roleManager.FindByNameAsync(roleName);

        if (role != null) {
            return role;
        }

        role = new AppRole(roleName);
        await _roleManager.CreateAsync(role);

        return role;
    }
    
    private async Task InitializeSuperAdminRole() {

        var superAdmin = _userManager.Users.FirstOrDefault(u => u.UserName == "root@health-tracking.com") ?? RegisterSuperAdmin();

        var superAdminRole = await RegisterRoleAsync(CustomRoles.SuperAdmin);

        if(!await _userManager.IsInRoleAsync(superAdmin, CustomRoles.SuperAdmin))
            await _userManager.AddToRoleAsync(superAdmin, superAdminRole.Name);
    }
    
    private AppUser RegisterSuperAdmin() {

        var superAdmin = new AppUser() {
            Id = Guid.NewGuid().ToString(),
            UserName = "root@health-tracking.com",
            Email = "root@health-tracking.com"
        };

        _userManager.CreateAsync(superAdmin, "_QGrXyvcmTD4aVQJ_").Wait();
        _userRepository.AddAsync(new User()
        {
            Id = superAdmin.Id,
            FirstName = "root",
            LastName = "root",
            Patronymic = "root",
            Email = "root@health-tracking.com",
            Phone = "000"
        }).Wait();

        return superAdmin;
    }
}
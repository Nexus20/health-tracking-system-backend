using AutoMapper;
using HealthTrackingSystem.Application.Authorization;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Identity;
using HealthTrackingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HealthTrackingSystem.Infrastructure.Repositories;

public class HospitalAdministratorRepository : RepositoryBase<HospitalAdministrator>, IHospitalAdministratorRepository
{
    private readonly ILogger<HospitalAdministratorRepository> _logger;
    private readonly UserManager<AppUser> _userManager;

    public HospitalAdministratorRepository(ApplicationDbContext dbContext, ILogger<HospitalAdministratorRepository> logger,
        UserManager<AppUser> userManager, IMapper mapper) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task AddAsync(HospitalAdministrator hospitalAdministratorEntity, User userEntity, string password)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.HospitalAdministrators.AddAsync(hospitalAdministratorEntity);
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating HospitalAdministrator account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.HospitalAdministrator,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting HospitalAdministrator account roles");
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new HospitalAdministrator request: {EMessage}", e.Message);
        }
    }
}
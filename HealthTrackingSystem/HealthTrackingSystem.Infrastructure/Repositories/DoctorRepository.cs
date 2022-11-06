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

public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
{
    private readonly ILogger<DoctorRepository> _logger;
    private readonly UserManager<AppUser> _userManager;

    public DoctorRepository(ApplicationDbContext dbContext, ILogger<DoctorRepository> logger,
        UserManager<AppUser> userManager, IMapper mapper) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task AddAsync(Doctor doctorEntity, User userEntity, string password)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.Doctors.AddAsync(doctorEntity);
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating doctor account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.Doctor,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting doctor account roles");
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new doctor request: {EMessage}", e.Message);
        }
    }
}
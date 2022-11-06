using AutoMapper;
using HealthTrackingSystem.Application.Authorization;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Identity;
using HealthTrackingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace HealthTrackingSystem.Infrastructure.Repositories;

public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
{
    private readonly Serilog.ILogger _logger;
    private readonly UserManager<AppUser> _userManager;
    
    public PatientRepository(ApplicationDbContext dbContext, IMapper mapper, Serilog.ILogger logger, UserManager<AppUser> userManager) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task AddAsync(Patient patientEntity, User userEntity, string password)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.Patients.AddAsync(patientEntity);
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating patient account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.Patient,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting patient account roles");
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.Error("Error while creating a new patient request: {EMessage}", e.Message);
        }
    }
}
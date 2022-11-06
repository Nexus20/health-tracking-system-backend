using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HealthTrackingSystem.Application.Authorization;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Domain.Entities.Abstract;
using HealthTrackingSystem.Infrastructure.Identity;
using HealthTrackingSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthTrackingSystem.Infrastructure.Repositories;

public class PatientCaretakerRepository : RepositoryBase<PatientCaretaker>, IPatientCaretakerRepository
{
    private readonly ILogger<PatientCaretakerRepository> _logger;
    private readonly UserManager<AppUser> _userManager;

    public PatientCaretakerRepository(ApplicationDbContext dbContext, ILogger<PatientCaretakerRepository> logger,
        UserManager<AppUser> userManager, IMapper mapper) : base(dbContext, mapper)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public Task<List<PatientCaretakerResult>> GetWithPatientsAsync(Expression<Func<PatientCaretaker, bool>>? predicate = null)
    {
        var query = DbContext.PatientCaretakers
            .Include(x => x.User)
            .Include(x => x.Patients)
            .ThenInclude(x => x.User)
            .AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return query.AsNoTracking()
            .ProjectTo<PatientCaretakerResult>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task AddAsync(PatientCaretaker patientCaretakerEntity, string password,
        List<string>? patientsIds)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync();
        try
        {
            var userEntity = patientCaretakerEntity.User;
            await DbContext.DomainUsers.AddAsync(userEntity);
            await DbContext.PatientCaretakers.AddAsync(patientCaretakerEntity);
            var appUser = Mapper.Map<User, AppUser>(userEntity);

            var identityResult = await _userManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while creating PatientCaretaker account");

            identityResult = await _userManager.AddToRolesAsync(appUser, new List<string>()
            {
                CustomRoles.User,
                CustomRoles.PatientCaretaker,
            });

            if (!identityResult.Succeeded)
                throw new IdentityException("Error while adjusting PatientCaretaker account roles");

            if (patientsIds?.Any() == true)
            {
                var patientsToUpdate = await DbContext.Patients.Where(x => patientsIds.Contains(x.Id)).ToListAsync();
                patientCaretakerEntity.Patients = patientsToUpdate;
            }
            
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new PatientCaretaker request: {EMessage}", e.Message);
        }
    }
}
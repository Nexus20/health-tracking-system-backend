using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class HospitalAdministratorService : IHospitalAdministratorService
{
    private readonly IHospitalAdministratorRepository _hospitalAdministratorRepository;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public HospitalAdministratorService(IHospitalAdministratorRepository hospitalAdministratorRepository, IMapper mapper, Serilog.ILogger logger, IHospitalRepository hospitalRepository)
    {
        _hospitalAdministratorRepository = hospitalAdministratorRepository;
        _mapper = mapper;
        _logger = logger;
        _hospitalRepository = hospitalRepository;
    }

    public async Task<HospitalAdministratorResult> GetByIdAsync(string id)
    {
        var source = await _hospitalAdministratorRepository.GetByIdAsync(id);

        if (source == null)
        {
            throw new NotFoundException($"HospitalAdministrator with id \"{id}\" not found");
        }

        return _mapper.Map<HospitalAdministrator, HospitalAdministratorResult>(source);
    }

    public async Task<List<HospitalAdministratorResult>> GetAsync(GetHospitalAdministratorsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _hospitalAdministratorRepository.GetAsync(predicate);

        return _mapper.Map<List<HospitalAdministrator>, List<HospitalAdministratorResult>>(source);
    }
    
    public async Task<HospitalAdministratorResult> CreateAsync(CreateHospitalAdministratorRequest request)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == request.HospitalId))
            throw new ValidationException($"Hospital with id {request.HospitalId} doesn't exist");

        var hospitalAdministratorExists = await _hospitalAdministratorRepository.ExistsAsync(x => x.User.FirstName == request.FirstName
                                                                    && x.User.LastName == request.LastName
                                                                    && x.User.Patronymic == request.Patronymic
                                                                    && x.User.Phone == request.Phone);

        if (hospitalAdministratorExists)
            throw new ValidationException("HospitalAdministrator with such parameters already exists");
        
        // var userEntity = _mapper.Map<CreateHospitalAdministratorRequest, User>(request);
        var hospitalAdministratorEntity = _mapper.Map<CreateHospitalAdministratorRequest, HospitalAdministrator>(request);
        // HospitalAdministratorEntity.UserId = userEntity.Id;
        await _hospitalAdministratorRepository.AddAsync(hospitalAdministratorEntity, hospitalAdministratorEntity.User, request.Password);
        _logger.Information("New HospitalAdministrator {@Entity} was created successfully", hospitalAdministratorEntity);
        var result = _mapper.Map<HospitalAdministrator, HospitalAdministratorResult>(hospitalAdministratorEntity); 
        return result;
    }

    public Task<HospitalAdministratorResult> UpdateAsync(UpdateHospitalAdministratorRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
    
    private Expression<Func<HospitalAdministrator, bool>>? CreateFilterPredicate(GetHospitalAdministratorsRequest request)
    {
        Expression<Func<HospitalAdministrator, bool>>? predicate = null;

        // if (!string.IsNullOrWhiteSpace(request.SearchString))
        // {
        //     Expression<Func<HelpRequest, bool>> searchStringExpression = x =>
        //         x.Description != null && x.Description.Contains(request.SearchString) ||
        //         x.Name.Contains(request.SearchString);
        //
        //     predicate = ExpressionsHelper.And(predicate, searchStringExpression);
        // }
        //
        // if (request.Status.HasValue && Enum.IsDefined(request.Status.Value))
        // {
        //     Expression<Func<HelpRequest, bool>> statusPredicate = x => x.Status == request.Status.Value;
        //     predicate = ExpressionsHelper.And(predicate, statusPredicate);
        // }
        //
        // if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate < request.EndDate)
        // {
        //     Expression<Func<HelpRequest, bool>> dateExpression = x => x.CreatedDate > request.StartDate.Value
        //                                                               && x.CreatedDate < request.EndDate.Value;
        //     predicate = ExpressionsHelper.And(predicate, dateExpression);
        // }

        return predicate;
    }
}
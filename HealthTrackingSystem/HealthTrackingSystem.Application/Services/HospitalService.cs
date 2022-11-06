using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class HospitalService : IHospitalService
{
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public HospitalService(IHospitalRepository hospitalRepository, IMapper mapper, Serilog.ILogger logger)
    {
        _hospitalRepository = hospitalRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<HospitalResult> GetByIdAsync(string id)
    {
        var source = await _hospitalRepository.GetByIdAsync(id);

        if (source == null)
        {
            throw new NotFoundException($"Hospital with id \"{id}\" not found");
        }

        return _mapper.Map<Hospital, HospitalResult>(source);
    }

    public Task<List<HospitalResult>> GetAsync(GetHospitalsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        return _hospitalRepository.GetAsync<HospitalResult>(predicate);
    }

    public async Task<HospitalResult> CreateAsync(CreateHospitalRequest request)
    {
        var existingHospital = await _hospitalRepository.GetAsync(x => x.Name == request.Name);

        if (existingHospital?.Any() == true)
        {
            throw new ValidationException($"Hospital with name {request.Name} already exists");
        }

        var entity = _mapper.Map<CreateHospitalRequest, Hospital>(request);
        await _hospitalRepository.AddAsync(entity);
        _logger.Information("New hospital {@Entity} was created successfully", entity);
        var result = _mapper.Map<Hospital, HospitalResult>(entity); 
        return result;
    }

    public Task<HospitalResult> UpdateAsync(UpdateHospitalRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
    
    private Expression<Func<Hospital, bool>>? CreateFilterPredicate(GetHospitalsRequest request)
    {
        Expression<Func<Hospital, bool>>? predicate = null;

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
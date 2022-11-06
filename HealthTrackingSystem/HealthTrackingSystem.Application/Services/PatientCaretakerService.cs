using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class PatientCaretakerService : IPatientCaretakerService
{
    private readonly IPatientCaretakerRepository _patientCaretakerRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public PatientCaretakerService(IPatientCaretakerRepository patientCaretakerRepository, IMapper mapper, Serilog.ILogger logger, IPatientRepository patientRepository)
    {
        _patientCaretakerRepository = patientCaretakerRepository;
        _mapper = mapper;
        _logger = logger;
        _patientRepository = patientRepository;
    }

    public async Task<PatientCaretakerResult> GetByIdAsync(string id)
    {
        var source = await _patientCaretakerRepository.GetByIdAsync(id);

        if (source == null)
        {
            throw new NotFoundException($"PatientCaretaker with id \"{id}\" not found");
        }

        return _mapper.Map<PatientCaretaker, PatientCaretakerResult>(source);
    }

    public async Task<List<PatientCaretakerResult>> GetAsync(GetPatientCaretakersRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var result = await _patientCaretakerRepository.GetWithPatientsAsync(predicate);
        return result;
    }
    
    public async Task<PatientCaretakerResult> CreateAsync(CreatePatientCaretakerRequest request)
    {
        var patientCaretakerExists = await _patientCaretakerRepository.ExistsAsync(x => x.User.FirstName == request.FirstName
                                                                    && x.User.LastName == request.LastName
                                                                    && x.User.Patronymic == request.Patronymic
                                                                    && x.User.Phone == request.Phone);

        if (patientCaretakerExists)
            throw new ValidationException("PatientCaretaker with such parameters already exists");

        if (request.PatientsIds?.Any() == true)
        {
            var patientsCount = await _patientRepository.CountAsync(x => request.PatientsIds.Contains(x.Id));

            if (patientsCount != request.PatientsIds.Count)
                throw new ValidationException("Invalid patients ids");
        }
        
        var patientCaretakerEntity = _mapper.Map<CreatePatientCaretakerRequest, PatientCaretaker>(request);
        await _patientCaretakerRepository.AddAsync(patientCaretakerEntity, request.Password, request.PatientsIds);
        
        _logger.Information("New PatientCaretaker {@Entity} was created successfully", patientCaretakerEntity);
        
        var result = _mapper.Map<PatientCaretaker, PatientCaretakerResult>(patientCaretakerEntity); 
        return result;
    }

    public Task<PatientCaretakerResult> UpdateAsync(UpdatePatientCaretakerRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
    
    private Expression<Func<PatientCaretaker, bool>>? CreateFilterPredicate(GetPatientCaretakersRequest request)
    {
        Expression<Func<PatientCaretaker, bool>>? predicate = null;

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
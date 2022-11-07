using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using HealthTrackingSystem.Application.Mqtt;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly IMqttSubscribersPool _mqttSubscribersPool;

    public PatientService(IPatientRepository patientRepository, IMapper mapper, Serilog.ILogger logger, IHospitalRepository hospitalRepository, IMqttSubscribersPool mqttSubscribersPool)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
        _logger = logger;
        _hospitalRepository = hospitalRepository;
        _mqttSubscribersPool = mqttSubscribersPool;
    }

    public async Task<PatientResult> GetByIdAsync(string id)
    {
        var result = await _patientRepository.GetByIdAsync<PatientResult>(id);

        if (result == null)
        {
            throw new NotFoundException($"Patient with id \"{id}\" not found");
        }

        return result;
    }

    public async Task<List<PatientResult>> GetAsync(GetPatientsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var result = await _patientRepository.GetAsync<PatientResult>(predicate);
        return result;
    }
    
    public async Task<PatientResult> CreateAsync(CreatePatientRequest request)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == request.HospitalId))
            throw new ValidationException($"Hospital with id {request.HospitalId} doesn't exist");

        var patientExists = await _patientRepository.ExistsAsync(x => x.User.FirstName == request.FirstName
                                                                    && x.User.LastName == request.LastName
                                                                    && x.User.Patronymic == request.Patronymic
                                                                    && x.User.Phone == request.Phone);

        if (patientExists)
            throw new ValidationException("Patient with such parameters already exists");
        
        var patientEntity = _mapper.Map<CreatePatientRequest, Patient>(request);
        await _patientRepository.AddAsync(patientEntity, patientEntity.User, request.Password);
        _logger.Information("New Patient {@Entity} was created successfully", patientEntity);
        var result = _mapper.Map<Patient, PatientResult>(patientEntity); 
        return result;
    }

    public Task<PatientResult> UpdateAsync(UpdatePatientRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateIotDeviceSubscriberForPatientAsync(string id)
    {
        _mqttSubscribersPool.AddNewSubscriber(id);
        await _mqttSubscribersPool.ConnectOneAsync(id);
        _logger.Information("IoT device subscriber for patient {Id} connected successfully", id);
    }

    private Expression<Func<Patient, bool>>? CreateFilterPredicate(GetPatientsRequest request)
    {
        Expression<Func<Patient, bool>>? predicate = null;

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
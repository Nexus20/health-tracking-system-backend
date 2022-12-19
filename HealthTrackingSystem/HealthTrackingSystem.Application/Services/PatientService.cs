using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
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
    private readonly IIotSubscribersPool _iotSubscribersPool;
    private readonly IDoctorRepository _doctorRepository;

    public PatientService(IPatientRepository patientRepository, IMapper mapper, Serilog.ILogger logger, IHospitalRepository hospitalRepository, IIotSubscribersPool iotSubscribersPool, IDoctorRepository doctorRepository)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
        _logger = logger;
        _hospitalRepository = hospitalRepository;
        _iotSubscribersPool = iotSubscribersPool;
        _doctorRepository = doctorRepository;
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

        if (!string.IsNullOrWhiteSpace(request.DoctorId))
        {
            if (!await _doctorRepository.ExistsAsync(x => x.Id == request.DoctorId))
                throw new ValidationException($"Doctor with id {request.DoctorId} doesn't exist");
        }

        var patientEntity = _mapper.Map<CreatePatientRequest, Patient>(request);
        await _patientRepository.AddAsync(patientEntity, patientEntity.User, request.Password);
        _logger.Information("New Patient {@Entity} was created successfully", patientEntity);
        var result = _mapper.Map<Patient, PatientResult>(patientEntity); 
        return result;
    }

    public async Task<PatientResult> UpdateAsync(string id, UpdatePatientRequest request)
    {
        var patientToUpdate = await _patientRepository.GetByIdAsync(id);
        
        if(patientToUpdate == null)
            throw new NotFoundException($"Patient with id \"{id}\" not found");

        _mapper.Map(request, patientToUpdate);
        await _patientRepository.UpdateAsync(patientToUpdate);
        var result = _mapper.Map<Patient, PatientResult>(patientToUpdate); 
        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var patientToDelete = await _patientRepository.GetByIdAsync(id);
        
        if(patientToDelete == null)
            throw new NotFoundException($"Patient with id \"{id}\" not found");

        await _patientRepository.DeleteAsync(patientToDelete);
    }

    public async Task CreateIotDeviceSubscriberForPatientAsync(string id)
    {
        _iotSubscribersPool.AddNewSubscriber(id);
        await _iotSubscribersPool.ConnectOneAsync(id);
        _logger.Information("IoT device subscriber for patient {Id} connected successfully", id);
    }

    public async Task<string> AddDoctorToPatientAsync(string id, AddDoctorToPatientRequest request)
    {
        var patientToUpdate = await _patientRepository.GetByIdAsync(id);
        
        if(patientToUpdate == null)
            throw new NotFoundException($"Patient with id \"{id}\" not found");

        if (!await _doctorRepository.ExistsAsync(x => x.Id == request.DoctorId))
            throw new ValidationException($"Doctor with id \"{request.DoctorId}\" not found");

        patientToUpdate.DoctorId = request.DoctorId;
        await _patientRepository.UpdateAsync(patientToUpdate);
        _logger.Information("Doctor {@DoctorId} has been added to patient {@PatientId} successfully", request.DoctorId, id);
        return request.DoctorId;
    }

    public async Task<PatientCaretakerResult?> GetPatientCaretakerAsync(string id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        
        if(patient == null)
            throw new NotFoundException($"Patient with id \"{id}\" not found");

        if (patient.PatientCaretaker == null)
            return null;

        var result = _mapper.Map<PatientCaretaker, PatientCaretakerResult>(patient.PatientCaretaker);
        return result;
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
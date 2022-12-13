using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Patients;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class HospitalService : IHospitalService
{
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IHospitalAdministratorRepository _hospitalAdministratorRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public HospitalService(IHospitalRepository hospitalRepository, IMapper mapper, Serilog.ILogger logger, IHospitalAdministratorRepository hospitalAdministratorRepository, IDoctorRepository doctorRepository, IPatientRepository patientRepository)
    {
        _hospitalRepository = hospitalRepository;
        _mapper = mapper;
        _logger = logger;
        _hospitalAdministratorRepository = hospitalAdministratorRepository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
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

    public async Task<HospitalResult> UpdateAsync(string id, UpdateHospitalRequest request)
    {
        var hospitalToUpdate = await _hospitalRepository.GetByIdAsync(id);

        if (hospitalToUpdate == null)
            throw new NotFoundException(nameof(Hospital), id);
        
        if (await _hospitalRepository.ExistsAsync(x => x.Name == request.Name && x.Id != id))
            throw new ValidationException("Hospital with such name already exists");

        if (await _hospitalRepository.ExistsAsync(x => x.Address == request.Address && x.Id != id))
            throw new ValidationException("Hospital with such address already exists");

        _mapper.Map<UpdateHospitalRequest, Hospital>(request, hospitalToUpdate);

        await _hospitalRepository.UpdateAsync(hospitalToUpdate);
        _logger.Information("Hospital with id {Id} has been updated successfully", hospitalToUpdate.Id);
        var result = _mapper.Map<Hospital, HospitalResult>(hospitalToUpdate);
        return result;
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<HospitalAdministratorResult>> GetAdministratorsAsync(string id)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == id))
            throw new NotFoundException(nameof(Hospital), id);
        
        var source = await _hospitalAdministratorRepository
            .GetAsync(x => x.HospitalId == id, disableTracking: false);

        var result = _mapper.Map<List<HospitalAdministrator>, List<HospitalAdministratorResult>>(source);
        return result;
    }

    public async Task<List<DoctorResult>> GetDoctorsAsync(string id)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == id))
            throw new NotFoundException(nameof(Hospital), id);
        
        var source = await _doctorRepository
            .GetAsync(x => x.HospitalId == id, disableTracking: false);

        var result = _mapper.Map<List<Doctor>, List<DoctorResult>>(source);
        return result;
    }

    public async Task<List<PatientResult>> GetPatientsAsync(string id)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == id))
            throw new NotFoundException(nameof(Hospital), id);
        
        var source = await _patientRepository
            .GetAsync(x => x.HospitalId == id, disableTracking: false);

        var result = _mapper.Map<List<Patient>, List<PatientResult>>(source);
        return result;
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
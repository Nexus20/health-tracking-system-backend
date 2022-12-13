using System.Linq.Expressions;
using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Doctors;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, Serilog.ILogger logger, IHospitalRepository hospitalRepository)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
        _logger = logger;
        _hospitalRepository = hospitalRepository;
    }

    public async Task<DoctorResult> GetByIdAsync(string id)
    {
        var source = await _doctorRepository.GetByIdAsync(id);

        if (source == null)
        {
            throw new NotFoundException($"Doctor with id \"{id}\" not found");
        }

        return _mapper.Map<Doctor, DoctorResult>(source);
    }

    public async Task<List<DoctorResult>> GetAsync(GetDoctorsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _doctorRepository.GetAsync(predicate);

        return _mapper.Map<List<Doctor>, List<DoctorResult>>(source);
    }
    
    public async Task<DoctorResult> CreateAsync(CreateDoctorRequest request)
    {
        if (!await _hospitalRepository.ExistsAsync(x => x.Id == request.HospitalId))
            throw new ValidationException($"Hospital with id {request.HospitalId} doesn't exist");

        var doctorExists = await _doctorRepository.ExistsAsync(x => x.User.FirstName == request.FirstName
                                                                    && x.User.LastName == request.LastName
                                                                    && x.User.Patronymic == request.Patronymic
                                                                    && x.User.Phone == request.Phone);

        if (doctorExists)
            throw new ValidationException("Doctor with such parameters already exists");
        
        var doctorEntity = _mapper.Map<CreateDoctorRequest, Doctor>(request);
        await _doctorRepository.AddAsync(doctorEntity, doctorEntity.User, request.Password);
        _logger.Information("New Doctor {@Entity} was created successfully", doctorEntity);
        var result = _mapper.Map<Doctor, DoctorResult>(doctorEntity); 
        return result;
    }

    public async Task<DoctorResult> UpdateAsync(string id, UpdateDoctorRequest request)
    {
        var doctorToUpdate = await _doctorRepository.GetByIdAsync(id);
        
        if(doctorToUpdate == null)
            throw new NotFoundException($"Doctor with id \"{id}\" not found");

        _mapper.Map(request, doctorToUpdate);
        await _doctorRepository.UpdateAsync(doctorToUpdate);
        var result = _mapper.Map<Doctor, DoctorResult>(doctorToUpdate); 
        return result;
    }

    public async Task DeleteAsync(string id)
    {
        var doctorToDelete = await _doctorRepository.GetByIdAsync(id);
        
        if(doctorToDelete == null)
            throw new NotFoundException($"Doctor with id \"{id}\" not found");

        await _doctorRepository.DeleteAsync(doctorToDelete);
    }
    
    private Expression<Func<Doctor, bool>>? CreateFilterPredicate(GetDoctorsRequest request)
    {
        Expression<Func<Doctor, bool>>? predicate = null;

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
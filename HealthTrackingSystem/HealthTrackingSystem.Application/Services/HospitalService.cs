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

    public HospitalService(IHospitalRepository hospitalRepository, IMapper mapper)
    {
        _hospitalRepository = hospitalRepository;
        _mapper = mapper;
    }

    public async Task<HospitalResult> GetByIdAsync(string id)
    {
        var source = await _hospitalRepository.GetByIdAsync(id);

        if (source == null)
        {
            throw new EntityNotFoundException($"Hospital with id \"{id}\" not found");
        }

        return _mapper.Map<Hospital, HospitalResult>(source);
    }

    public async Task<List<HospitalResult>> GetAsync(GetHospitalsRequest request)
    {
        var source = await _hospitalRepository.GetAllAsync();

        return _mapper.Map<List<Hospital>, List<HospitalResult>>(source);
    }

    public Task<HospitalResult> CreateHospitalAsync(CreateHospitalRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<HospitalResult> UpdateHospitalAsync(UpdateHospitalRequest request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteHospitalAsync(string id)
    {
        throw new NotImplementedException();
    }
}
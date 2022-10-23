using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HospitalController : ControllerBase
{
    private readonly IHospitalService _hospitalService;

    public HospitalController(IHospitalService hospitalService)
    {
        _hospitalService = hospitalService;
    }
    
    [HttpGet("{id}", Name = "GetHospitalById")]
    [ProducesResponseType(typeof(HospitalResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<HospitalResult>> GetById(string id)
    {
        var hospital = await _hospitalService.GetByIdAsync(id);
        return Ok(hospital);
    }
    
    [HttpGet(Name = "GetHospitals")]
    [ProducesResponseType(typeof(IEnumerable<HospitalResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HospitalResult>>> Get([FromQuery]GetHospitalsRequest request)
    {
        var hospitals = await _hospitalService.GetAsync(request);
        return Ok(hospitals);
    }
}
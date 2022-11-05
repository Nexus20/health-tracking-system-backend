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
    [ProducesResponseType(typeof(List<HospitalResult>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<HospitalResult>>> Get([FromQuery]GetHospitalsRequest request)
    {
        var hospitals = await _hospitalService.GetAsync(request);
        return Ok(hospitals);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(HospitalResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateHospitalRequest request)
    {
        var result = await _hospitalService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }
}
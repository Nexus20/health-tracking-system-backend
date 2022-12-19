using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Hospitals;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [HttpGet("{id}/administrators", Name = "Get administrators of this hospital")]
    public async Task<IActionResult> GetHospitalAdministrators(string id)
    {
        var administrators = await _hospitalService.GetAdministratorsAsync(id);
        return Ok(administrators);
    }
    
    [HttpGet("{id}/doctors", Name = "Get doctors of this hospital")]
    public async Task<IActionResult> GetHospitalDoctors(string id)
    {
        var administrators = await _hospitalService.GetDoctorsAsync(id);
        return Ok(administrators);
    }
    
    [HttpGet("{id}/patients", Name = "Get patients of this hospital")]
    public async Task<IActionResult> GetHospitalPatients(string id)
    {
        var administrators = await _hospitalService.GetPatientsAsync(id);
        return Ok(administrators);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HospitalResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateHospitalRequest request)
    {
        var result = await _hospitalService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    // PUT: api/Hospital/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] UpdateHospitalRequest request)
    {
        var result = await _hospitalService.UpdateAsync(id, request);
        return Ok(result);
    }

    // DELETE: api/Hospital/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _hospitalService.DeleteAsync(id);
        return NoContent();
    }
}
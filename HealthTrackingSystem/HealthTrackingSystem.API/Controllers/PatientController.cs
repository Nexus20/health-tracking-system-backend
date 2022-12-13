using System.Collections.Generic;
using System.Threading.Tasks;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IConnectionMultiplexer _redis;

        public PatientController(IPatientService patientService, IConnectionMultiplexer redis)
        {
            _patientService = patientService;
            _redis = redis;
        }

        [HttpGet("{id}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientResult>> GetById(string id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            return Ok(patient);
        }
        
        [HttpGet("{id}/measurements", Name = "GetMeasurementsById")]
        [ProducesResponseType(typeof(PatientResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeasurementsById(string id)
        {
            var result = await _redis.GetDatabase().SetMembersAsync($"patient:{id}"); 
            return Ok(result);
        }
    
        [HttpGet(Name = "GetPatients")]
        [ProducesResponseType(typeof(List<PatientResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientResult>>> Get([FromQuery]GetPatientsRequest request)
        {
            var patients = await _patientService.GetAsync(request);
            return Ok(patients);
        }
    
        [HttpPost]
        [ProducesResponseType(typeof(PatientResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreatePatientRequest request)
        {
            var result = await _patientService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, result);
        }
        
        [HttpGet("{id}/[action]", Name = "SubscribePatientToIotDevice")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SubscribePatientToIotDevice(string id)
        {
            await _patientService.CreateIotDeviceSubscriberForPatientAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdatePatientRequest request)
        {
            var result = await _patientService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}

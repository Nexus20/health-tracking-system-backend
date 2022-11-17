using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IConnectionMultiplexer _redis;

        public PatientsController(IPatientService patientService, IConnectionMultiplexer redis)
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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

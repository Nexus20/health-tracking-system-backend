using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("{id}", Name = "GetPatientById")]
        [ProducesResponseType(typeof(PatientResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientResult>> GetById(string id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            return Ok(patient);
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

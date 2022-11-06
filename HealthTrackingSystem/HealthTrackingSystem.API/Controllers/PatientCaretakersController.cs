using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientCaretakersController : ControllerBase
    {
        private readonly IPatientCaretakerService _patientCaretakerService;

        public PatientCaretakersController(IPatientCaretakerService patientCaretakerService)
        {
            _patientCaretakerService = patientCaretakerService;
        }

        [HttpGet("{id}", Name = "GetPatientCaretakerById")]
        [ProducesResponseType(typeof(PatientCaretakerResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<PatientCaretakerResult>> GetById(string id)
        {
            var patientCaretaker = await _patientCaretakerService.GetByIdAsync(id);
            return Ok(patientCaretaker);
        }
    
        [HttpGet(Name = "GetPatientCaretakers")]
        [ProducesResponseType(typeof(List<PatientCaretakerResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientCaretakerResult>>> Get([FromQuery]GetPatientCaretakersRequest request)
        {
            var patientCaretakers = await _patientCaretakerService.GetAsync(request);
            return Ok(patientCaretakers);
        }
    
        [HttpPost]
        [ProducesResponseType(typeof(PatientCaretakerResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreatePatientCaretakerRequest request)
        {
            var result = await _patientCaretakerService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        // PUT: api/PatientCaretakers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/PatientCaretakers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

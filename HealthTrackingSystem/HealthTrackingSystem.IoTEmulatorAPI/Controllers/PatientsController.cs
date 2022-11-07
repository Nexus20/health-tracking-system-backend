using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using HealthTrackingSystem.IoTEmulatorAPI.IoT;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace HealthTrackingSystem.IoTEmulatorAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IIotPublishersPool _iotPublishersPool;
        private readonly Serilog.ILogger _logger;

        public PatientsController(IPatientService patientService, IIotPublishersPool iotPublishersPool, ILogger logger)
        {
            _patientService = patientService;
            _iotPublishersPool = iotPublishersPool;
            _logger = logger;
        }

        [HttpGet(Name = "GetPatients")]
        [ProducesResponseType(typeof(List<PatientResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientResult>>> Get([FromQuery]GetPatientsRequest request)
        {
            var patients = await _patientService.GetAsync(request);
            return Ok(patients);
        }

        [HttpGet("{id}/[action]", Name = "SubscribePatientToIotDevice")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SubscribePatientToIotDevice(string id)
        {
            _iotPublishersPool.AddNewPublisher(id);
            await _iotPublishersPool.ConnectOneAsync(id);
            _logger.Information("IoT device publisher for patient {Id} connected successfully", id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}

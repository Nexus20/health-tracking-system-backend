using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.Doctors;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{id}", Name = "GetDoctorById")]
        [ProducesResponseType(typeof(DoctorResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<DoctorResult>> GetById(string id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            return Ok(doctor);
        }

        [HttpGet(Name = "GetDoctors")]
        [ProducesResponseType(typeof(List<DoctorResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DoctorResult>>> Get([FromQuery]GetDoctorsRequest request)
        {
            var doctors = await _doctorService.GetAsync(request);
            return Ok(doctors);
        }
    
        [HttpPost]
        [ProducesResponseType(typeof(DoctorResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateDoctorRequest request)
        {
            var result = await _doctorService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        // PUT: api/Doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateDoctorRequest request)
        {
            var result = await _doctorService.UpdateAsync(id, request);
            return Ok(result);
        }
        
        [HttpGet("{id}/patients", Name = "Get patients of this doctor")]
        public async Task<IActionResult> GetPatients(string id)
        {
            var patients = await _doctorService.GetPatientsAsync(id);
            return Ok(patients);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _doctorService.DeleteAsync(id);
            return NoContent();
        }
    }
}

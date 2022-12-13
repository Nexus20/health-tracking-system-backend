using System.Collections.Generic;
using System.Threading.Tasks;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Requests.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthTrackingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalAdministratorController : ControllerBase
    {
        private readonly IHospitalAdministratorService _hospitalAdministratorService;

        public HospitalAdministratorController(IHospitalAdministratorService hospitalAdministratorService)
        {
            _hospitalAdministratorService = hospitalAdministratorService;
        }

        [HttpGet("{id}", Name = "GetHospitalAdministratorById")]
        [ProducesResponseType(typeof(HospitalAdministratorResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<HospitalAdministratorResult>> GetById(string id)
        {
            var hospitalAdministrator = await _hospitalAdministratorService.GetByIdAsync(id);
            return Ok(hospitalAdministrator);
        }
    
        [HttpGet(Name = "GetHospitalAdministrators")]
        [ProducesResponseType(typeof(List<HospitalAdministratorResult>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HospitalAdministratorResult>>> Get([FromQuery]GetHospitalAdministratorsRequest request)
        {
            var hospitalAdministrators = await _hospitalAdministratorService.GetAsync(request);
            return Ok(hospitalAdministrators);
        }
    
        [HttpPost]
        [ProducesResponseType(typeof(HospitalAdministratorResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateHospitalAdministratorRequest request)
        {
            var result = await _hospitalAdministratorService.CreateAsync(request);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        // PUT: api/HospitalAdministrators/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/HospitalAdministrators/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

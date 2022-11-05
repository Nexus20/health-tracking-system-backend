using System.ComponentModel.DataAnnotations;

namespace HealthTrackingSystem.Application.Models.Requests.Hospitals;

public class CreateHospitalRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
}
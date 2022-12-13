using System.ComponentModel.DataAnnotations;

namespace HealthTrackingSystem.Application.Models.Requests.Hospitals;

public class UpdateHospitalRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [Required]
    public string Address { get; set; } = null!;
}
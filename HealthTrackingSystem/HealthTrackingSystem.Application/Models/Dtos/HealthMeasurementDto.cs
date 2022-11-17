namespace HealthTrackingSystem.Application.Models.Dtos;

public class HealthMeasurementDto
{
    public double Ecg { get; set; }
    public int HeartRate { get; set; }
    public DateTime DateTime { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthTrackingSystem.Domain.Entities;

public class MongoHealthMeasurement
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    public DateTime CreatedDate { get; set; }
    
    public double Ecg { get; set; }
    public int HeartRate { get; set; }
    
    public string PatientId { get; set; } = null!;
}
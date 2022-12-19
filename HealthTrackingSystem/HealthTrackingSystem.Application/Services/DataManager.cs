using HealthTrackingSystem.Application.Models.Results.Charts;

namespace HealthTrackingSystem.Application.Services;

public class DataManager
{
    public static List<ChartModel> GetData()
    {
        var r = new Random();
        return new List<ChartModel>()
        {
            new() { Data = new List<int> { r.Next(1, 40) }, Label = "Data1", BackgroundColor = "#5491DA" },
            new() { Data = new List<int> { r.Next(1, 40) }, Label = "Data2", BackgroundColor = "#E74C3C" },
            new() { Data = new List<int> { r.Next(1, 40) }, Label = "Data3", BackgroundColor = "#82E0AA" },
            new() { Data = new List<int> { r.Next(1, 40) }, Label = "Data4", BackgroundColor = "#E5E7E9" }
        };
    }
}

public class EcgPoint
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class HeartRateModel
{
    public int HeartRate { get; set; }
}

public class EcgDataManager
{
    private static int _currentIndex = 0;

    private static readonly List<EcgPoint> Points = new List<EcgPoint>()
    {
        new() { X = 0, Y = 0},
        new() { X = 80, Y = 0},
        new() { X = 100, Y = 20},
        new() { X = 120, Y = 0},
        new() { X = 200, Y = 0},
        new() { X = 210, Y = -10},
        new() { X = 230, Y = 100},
        new() { X = 250, Y = -20},
        new() { X = 270, Y = 0},
        new() { X = 310, Y = 0},
        new() { X = 390, Y = 40},
        new() { X = 470, Y = 0},
        new() { X = 720, Y = 0},
    };

    public static EcgPoint GetPoint()
    {
        if (_currentIndex == Points.Count)
            _currentIndex = 0;
        
        var result = Points[_currentIndex];
        _currentIndex++;
        return result;
    }
}
namespace SolarWatchMinimal.Model;

public class Times
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public string? SunRiseTime { get; set; }
    public string? SunSetTime { get; set; }
    
    public Times(string? sunRiseTime, string? sunSetTime)
    {
        SunRiseTime = sunRiseTime;
        SunSetTime = sunSetTime;
    }
}
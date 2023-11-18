namespace SolarWatch.Model;

public class SunriseSunsetTimes
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public string? SunRiseTime { get; set; }
    public string? SunSetTime { get; set; }
    
    public SunriseSunsetTimes(string? sunRiseTime, string? sunSetTime)
    {
        SunRiseTime = sunRiseTime;
        SunSetTime = sunSetTime;
    }
    
    public void ChangeSunTimeData(string? sunRiseTime, string? sunSetTime)
    {
        SunRiseTime = sunRiseTime;
        SunSetTime = sunSetTime;
    }
}
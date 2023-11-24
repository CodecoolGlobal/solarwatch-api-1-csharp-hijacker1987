namespace SolarWatchMinimal.Controllers.Interfaces;

public interface ISolarDataProvider
{
    Task<string> GetCurrentAsync(string cityName);
    Task<string> GetCurrentAsync(double lat, double lon);
}
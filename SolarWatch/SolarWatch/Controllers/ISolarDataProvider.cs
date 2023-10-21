namespace SolarWatch.Controllers;

public interface ISolarDataProvider
{
    string GetCurrent(string cityName);
    string GetCurrent(float lat, float lon);
    Task<string> GetCurrentAsync(string cityName);
    Task<string> GetCurrentAsync(double lat, double lon);
}
namespace SolarWatch.Controllers;

public interface ISolarDataProvider
{
    string GetCurrent(string cityName);
    string GetCurrent(float lat, float lon);
}
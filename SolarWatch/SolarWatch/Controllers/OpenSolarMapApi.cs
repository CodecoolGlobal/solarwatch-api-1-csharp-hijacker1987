using System.Net;

namespace SolarWatch.Controllers;

public class OpenSolarMapApi : ApiKey, ISolarDataProvider
{
    private readonly ILogger<OpenSolarMapApi> _logger;
   
    public OpenSolarMapApi(ILogger<OpenSolarMapApi> logger)
    {
        _logger = logger;
    }

    public string GetCurrent(string cityName)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={Api}";

        var client = new WebClient();

        _logger.LogInformation("Calling Geocoding API with url: {url}", url);
        return client.DownloadString(url);
    }
    
    public string GetCurrent(float lat, float lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        var client = new WebClient();

        _logger.LogInformation("Calling Sunset and sunrise times API with url: {url}", url);
        return client.DownloadString(url);
    }
}
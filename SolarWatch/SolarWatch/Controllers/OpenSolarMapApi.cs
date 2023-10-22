using System.Net;

namespace SolarWatch.Controllers;

public class OpenSolarMapApi : ISolarDataProvider
{
    private readonly ILogger<OpenSolarMapApi> _logger;
    public string Api { get; }
   
    public OpenSolarMapApi(ILogger<OpenSolarMapApi> logger)
    {
        _logger = logger;
        Api = "1593b000aeea6d27a1247cb005f6103b";
    }
    
    public async Task<string> GetCurrentAsync(double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
    
    public async Task<string> GetCurrentAsync(string cityName)
    {
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={Api}";

        using var client = new HttpClient();
        _logger.LogInformation("Calling Geocoding API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
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